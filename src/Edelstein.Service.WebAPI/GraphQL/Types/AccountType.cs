using System;
using System.Linq;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Database.Entities;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountType : ObjectGraphType<Account>
    {
        public AccountType(WebAPIService service)
        {
            Name = "Account";

            Field("id", x => x.ID);
            Field(x => x.Username);
            Field(x => x.NexonCash);
            Field(x => x.MaplePoint);
            Field(x => x.PrepaidNXCash);

            Field<AccountStateType>("state", resolve: ctx =>
            {
                if (service.AccountStateCache.ExistsAsync(ctx.Source.ID.ToString()).Result)
                    return service.AccountStateCache.GetAsync<MigrationState>(ctx.Source.ID.ToString()).Result;
                return MigrationState.LoggedOut;
            });

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