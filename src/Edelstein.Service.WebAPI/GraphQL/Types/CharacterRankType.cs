using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class CharacterRankType : ObjectGraphType<RankRecord>
    {
        public CharacterRankType(WebAPIService service)
        {
            Name = "CharacterRank";

            Field(x => x.WorldRank);
            Field(x => x.WorldRankGap);
            Field(x => x.JobRank);
            Field(x => x.JobRankGap);

            Field<CharacterType>(
                "character",
                resolve: ctx =>
                {
                    using (var store = service.DataStore.OpenSession())
                    {
                        return store
                            .Query<Character>()
                            .FirstOrDefault(c => c.ID == ctx.Source.CharacterID);
                    }
                });
        }
    }
}