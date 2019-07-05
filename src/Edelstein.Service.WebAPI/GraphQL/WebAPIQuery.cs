using System;
using System.Linq;
using System.Security.Claims;
using Edelstein.Database.Entities;
using Edelstein.Service.WebAPI.GraphQL.Types;
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
                    var identity = (ClaimsIdentity) ctx.UserContext;
                    var accountID = Convert.ToInt32(identity.Claims
                        .Single(c => c.Type == ClaimTypes.Sid).Value);
                    using (var store = service.DataStore.OpenSession())
                    {
                        return store
                            .Query<Account>()
                            .First(a => a.ID == accountID);
                    }
                }
            );
        }
    }
}