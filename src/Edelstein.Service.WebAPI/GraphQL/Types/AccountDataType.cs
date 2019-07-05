using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountDataType : ObjectGraphType<AccountData>
    {
        public AccountDataType(WebAPIService service)
        {
            Name = "AccountData";

            Field<int>("worldID", x => x.WorldID);

            Field<ListGraphType<CharacterType>>(
                "characters",
                resolve: ctx =>
                {
                    using (var store = service.DataStore.OpenSession())
                    {
                        return store
                            .Query<Character>()
                            .Where(a => a.AccountDataID == ctx.Source.ID);
                    }
                });
        }
    }
}