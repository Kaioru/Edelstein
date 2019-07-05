using Edelstein.Core.Distributed.Migrations;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public class AccountStateType : EnumerationGraphType<MigrationState>
    {
        public AccountStateType()
        {
            AddValue("LoggedOut", "", 0);
            AddValue("LoggedIn", "", 1);
            AddValue("Migrating", "", 2);
        }
    }
}