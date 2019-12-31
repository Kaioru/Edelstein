using System.Threading.Tasks;
using Edelstein.Core.Services.Migrations;
using Edelstein.Network;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game
{
    public class GameServiceAdapter : AbstractMigrationSocketAdapter
    {
        public GameService Service { get; }
        public FieldUser User { get; set; }

        public GameServiceAdapter(
            ISocket socket,
            GameService service
        ) : base(socket, service)
            => Service = service;

        public override async Task OnDisconnect()
        {
            if (User != null) await User.Field.Leave(User);
            await base.OnDisconnect();
        }
    }
}