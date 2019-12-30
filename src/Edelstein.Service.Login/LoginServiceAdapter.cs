using Edelstein.Core.Distributed;
using Edelstein.Core.Services.Migrations;
using Edelstein.Network;

namespace Edelstein.Service.Login
{
    public class LoginServiceAdapter : AbstractMigrationSocketAdapter
    {
        public LoginService Service { get; }
        public INodeRemote SelectedNode { get; set; }

        public LoginServiceAdapter(
            ISocket socket,
            LoginService service
        ) : base(socket, service)
        {
            Service = service;
        }
    }
}