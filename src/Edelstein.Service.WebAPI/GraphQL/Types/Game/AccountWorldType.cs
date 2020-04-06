using Edelstein.Entities;
using Edelstein.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types.Game
{
    public sealed class AccountWorldType : ObjectGraphType<AccountWorld>
    {
        public AccountWorldType(WebAPIService service)
        {
            Name = "AccountWorld";

            Field<int>("worldID", x => x.WorldID);

            Field<ListGraphType<CharacterType>>(
                "characters",
                resolve: ctx =>
                {
                    using var store = service.DataStore.StartSession();
                    return store
                        .Query<Character>()
                        .Where(a => a.AccountWorldID == ctx.Source.ID)
                        .ToImmutableList();
                });
        }
    }
}