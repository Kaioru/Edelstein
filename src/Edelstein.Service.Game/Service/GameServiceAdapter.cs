using Edelstein.Core.Services.Migrations;
using Edelstein.Network;

namespace Edelstein.Service.Game.Service
{
    public class GameServiceAdapter : AbstractMigrationSocketAdapter
    {
        public GameServiceAdapter(
            ISocket socket,
            IMigrationService service
        ) : base(socket, service)
        {
        }
    }
}