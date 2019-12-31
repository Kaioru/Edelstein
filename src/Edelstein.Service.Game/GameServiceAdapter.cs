using Edelstein.Core.Services.Migrations;
using Edelstein.Network;

namespace Edelstein.Service.Game
{
    public class GameServiceAdapter : AbstractMigrationSocketAdapter
    {
        public GameService Service { get; }

        public GameServiceAdapter(
            ISocket socket,
            GameService service
        ) : base(socket, service)
            => Service = service;
    }
}