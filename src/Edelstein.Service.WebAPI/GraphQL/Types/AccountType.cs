using System.Linq;
using Edelstein.Database.Entities;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountType : ObjectGraphType<Account>
    {
        public AccountType(WebAPIService service)
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