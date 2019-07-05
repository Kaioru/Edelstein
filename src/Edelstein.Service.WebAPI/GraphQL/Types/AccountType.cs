using System.Linq;
using Edelstein.Database.Entities;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountType : ObjectGraphType<Account>
    {
        public AccountType()
        {
            Name = "Account";

            Field(x => x.ID);
            Field(x => x.Username);
            Field(x => x.NexonCash);
            Field(x => x.MaplePoint);
            Field(x => x.PrepaidNXCash);

            Field<ListGraphType<AccountDataType>>(
                "data",
                resolve: ctx =>
                {
                    var userCtx = (WebAPIContext) ctx.UserContext;
                    var service = userCtx.Service;

                    using (var store = service.DataStore.OpenSession())
                    {
                        return store
                            .Query<AccountData>()
                            .Where(a => a.AccountID == ctx.Source.ID);
                    }
                });
        }
    }
}