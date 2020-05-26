using System;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Core.Distributed;
using Edelstein.Core.Entities;
using Edelstein.Core.Entities.Characters;
using Edelstein.Core.Gameplay.Migrations;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types.Game
{
    public sealed class AccountType : ObjectGraphType<Account>
    {
        public AccountType(WebAPIService service)
        {
            Name = "Account";

            Field<int>("id", x => x.ID);
            Field<string>("username", x => x.Username);

            Field<AccountStateType>("currentState", resolve: ctx =>
                service.AccountStateCache.ExistsAsync(ctx.Source.ID.ToString()).Result
                    ? service.AccountStateCache.GetAsync<MigrationState>(ctx.Source.ID.ToString()).Result.Value
                    : MigrationState.LoggedOut
            );

            Field<CharacterType>("currentCharacter", resolve: ctx =>
            {
                using var store = service.DataStore.StartSession();
                var worldData = store
                    .Query<AccountWorld>()
                    .Where(a => a.AccountID == ctx.Source.ID)
                    .ToImmutableList()
                    .Select(a => a.ID)
                    .ToImmutableList();
                var characters = store
                    .Query<Character>()
                    .Where(c => worldData.Contains(c.AccountWorldID))
                    .ToImmutableList()
                    .Select(c => c.ID.ToString())
                    .ToImmutableList();
                var servers = service.CharacterStateCache.GetAllAsync<IServerNodeState>(characters).Result;
                var characterID = Convert.ToInt32(servers.FirstOrDefault(s => s.Value.HasValue).Key);

                return store
                    .Query<Character>()
                    .Where(c => c.ID == characterID)
                    .FirstOrDefault();
            });

            Field<ListGraphType<AccountWorldType>>(
                "worldData",
                resolve: ctx =>
                {
                    using var store = service.DataStore.StartSession();
                    return store
                        .Query<AccountWorld>()
                        .Where(a => a.AccountID == ctx.Source.ID)
                        .ToImmutableList();
                });
        }
    }
}