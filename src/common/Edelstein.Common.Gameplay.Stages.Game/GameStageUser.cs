using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStageUser : AbstractMigrateableStageUser<GameStage, GameStageUser, GameStageConfig>, IGameStageUser<GameStage, GameStageUser>
    {
        public IField Field => FieldUser.Field;
        public IFieldObjUser FieldUser { get; set; }

        public GameStageUser(ISocket socket) : base(socket) { }

        public override async Task OnDisconnect()
        {
            if (Character != null && Field != null && FieldUser != null)
            {
                Character.FieldID = Field.Template.FieldReturn ?? Field.ID;
                Character.FieldPortal = (byte)Field.GetStartPointClosestTo(FieldUser.Position).ID;
            }

            await base.OnDisconnect();
        }
    }
}
