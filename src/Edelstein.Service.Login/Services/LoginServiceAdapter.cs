using Edelstein.Core.Services.Migrations;
using Edelstein.Network;

namespace Edelstein.Service.Login.Services
{
    public class LoginServiceAdapter : AbstractMigrationSocketAdapter
    {
        public LoginServiceAdapter(
            ISocket socket,
            IMigrationService service
        ) : base(socket, service)
        {
        }
    }
}