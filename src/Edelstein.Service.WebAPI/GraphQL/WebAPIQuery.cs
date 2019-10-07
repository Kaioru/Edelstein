using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Service.WebAPI.GraphQL.Types;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPIQuery : ObjectGraphType
    {
        public WebAPIQuery(WebAPIService service)
        {
            Field<AccountType>(
                "account",
                resolve: ctx =>
                {
                    var accountID = ((WebAPIContext) ctx.UserContext).AccountID;
                    using var store = service.DataStore.OpenSession();
                    return store
                        .Query<Account>()
                        .First(a => a.ID == accountID);
                }
            ).AuthorizeWith("Authorized");
        }
    }
}