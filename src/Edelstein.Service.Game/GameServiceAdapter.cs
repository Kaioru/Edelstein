using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Migrations;
using Edelstein.Core.Templates.Fields;
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

        public override async Task OnUpdate()
        {
            if (User?.Field != null)
            {
                var template = User.Field.Template;

                if (template.ForcedReturn.HasValue)
                {
                    Character.FieldID = template.ForcedReturn.Value;
                    Character.FieldPortal = 0;
                }
                else
                    Character.FieldPortal = (byte) template.Portals
                        .Values
                        .Where(p => p.Type == FieldPortalType.StartPoint)
                        .OrderBy(p =>
                        {
                            var xd = p.Position.X - User.Position.X;
                            var yd = p.Position.Y - User.Position.Y;

                            return xd * xd + yd * yd;
                        })
                        .First()
                        .ID;
            }

            await base.OnUpdate();
        }

        public override async Task OnDisconnect()
        {
            await base.OnDisconnect();
            if (User != null) await User.Field.Leave(User);
        }
    }
}