using System;
using Edelstein.Core.Types;
using Edelstein.Database.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class CharacterType : ObjectGraphType<Character>
    {
        public CharacterType(WebAPIService service)
        {
            Name = "Character";

            Field("id", x => x.ID);
            Field(x => x.Name);
            Field<int>("gender", x => x.Gender);
            Field<int>("skin", x => x.Skin);
            Field(x => x.Face);
            Field(x => x.Hair);
            Field<int>("level", x => x.Level);
            Field<int>("job", x => x.Job);
            Field<string>("jobName", x => Enum.GetName(typeof(Job), x.Job));
            Field<int>("str", x => x.STR);
            Field<int>("dex", x => x.DEX);
            Field<int>("int", x => x.INT);
            Field<int>("luk", x => x.LUK);
            Field("hp", x => x.HP);
            Field(x => x.MaxHP);
            Field("mp", x => x.MP);
            Field(x => x.MaxMP);
            Field("exp", x => x.EXP);
            Field<int>("pop", x => x.POP);
            Field(x => x.Money);
        }
    }
}