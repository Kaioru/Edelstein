using System;
using Edelstein.Core.Distributed;
using Edelstein.Core.Gameplay;
using Edelstein.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types.Game
{
    public sealed class CharacterType : ObjectGraphType<Character>
    {
        public CharacterType(WebAPIService service)
        {
            Name = "Character";

            Field<int>("id", x => x.ID);
            Field<string>("name", x => x.Name);
            Field<int>("gender", x => x.Gender);
            Field<int>("skin", x => x.Skin);
            Field<int>("face", x => x.Face);
            Field<int>("hair", x => x.Hair);
            Field<int>("level", x => x.Level);
            Field<int>("job", x => x.Job);
            Field<string>("jobName", x => Enum.GetName(typeof(Job), x.Job));
            Field<int>("str", x => x.STR);
            Field<int>("dex", x => x.DEX);
            Field<int>("int", x => x.INT);
            Field<int>("luk", x => x.LUK);
            Field<int>("hp", x => x.HP);
            Field<int>("maxHp", x => x.MaxHP);
            Field<int>("mp", x => x.MP);
            Field<int>("maxMp", x => x.MaxMP);
            Field<int>("exp", x => x.EXP);
            Field<int>("pop", x => x.POP);
            Field<int>("money", x => x.Money);

            Field<ServerStateType>("currentServer", resolve: ctx =>
                service.CharacterStateCache.ExistsAsync(ctx.Source.ID.ToString()).Result
                    ? service.CharacterStateCache.GetAsync<IServerNodeState>(ctx.Source.ID.ToString()).Result.Value
                    : null
            );
        }
    }
}