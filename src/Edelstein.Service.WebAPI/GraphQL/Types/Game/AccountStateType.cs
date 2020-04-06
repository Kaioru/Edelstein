using Edelstein.Core.Gameplay.Migrations;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types.Game
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