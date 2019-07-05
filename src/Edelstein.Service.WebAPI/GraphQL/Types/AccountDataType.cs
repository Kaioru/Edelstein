using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountDataType : ObjectGraphType<AccountData>
    {
        public AccountDataType()
        {
            Name = "AccountData";

            Field<int>("worldID", x => x.WorldID);

            Field<ListGraphType<CharacterType>>(
                "characters",
                resolve: ctx =>
                {
                    var userCtx = (WebAPIContext) ctx.UserContext;
                    var service = userCtx.Service;

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