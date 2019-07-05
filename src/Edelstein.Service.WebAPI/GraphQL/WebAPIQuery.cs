using System.Linq;
using Edelstein.Database.Entities;
using Edelstein.Service.WebAPI.GraphQL.Types;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPIQuery : ObjectGraphType
    {
        public WebAPIQuery(WebAPIService service, int accountID)
        {
            Field<AccountType>(
                "account",
                resolve: ctx =>
                {
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