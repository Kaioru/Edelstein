using System.Collections.Immutable;
using System.Linq;
using Edelstein.Core.Distributed;
using Edelstein.Core.Entities;
using Edelstein.Service.WebAPI.GraphQL.Context;
using Edelstein.Service.WebAPI.GraphQL.Types;
using Edelstein.Service.WebAPI.GraphQL.Types.Game;
using GraphQL.Server.Authorization.AspNetCore;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPIQuery : ObjectGraphType
    {
        public WebAPIQuery(WebAPIService service)
        {
            Field<AccountType>(
                "currentAccount",
                resolve: ctx =>
                {
                    var accountID = ((WebAPIContext) ctx.UserContext).AccountID;
                    using var store = service.DataStore.StartSession();
                    return store
                        .Query<Account>()
                        .Where(a => a.ID == accountID)
                        .FirstOrDefault();
                }
            ).AuthorizeWith("Authorized");

            Field<ListGraphType<ServerStateType>>("servers", resolve: ctx =>
                service.GetPeers().Result
                    .Select(p => p.State)
                    .OfType<IServerNodeState>()
                    .OrderBy(s => s.Port)
                    .ToImmutableList()
            );
        }
    }
}