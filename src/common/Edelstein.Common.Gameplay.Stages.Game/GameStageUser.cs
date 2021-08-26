using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStageUser : AbstractServerStageUser<GameStage, GameStageUser, GameStageConfig>, IGameStageUser<GameStage, GameStageUser>
    {
        public IField Field => FieldUser?.Field;
        public IFieldObjUser FieldUser { get; set; }

        public GameStageUser(ISocket socket, IPacketProcessor<GameStage, GameStageUser> processor) : base(socket, processor) { }

        public override async Task OnDisconnect()
        {
            if (Character != null && Field != null && FieldUser != null)
            {
                Character.FieldID = Field.Info.FieldReturn ?? Field.ID;
                Character.FieldPortal = (byte)Field.GetStartPointClosestTo(FieldUser.Position).ID;

                await FieldUser.EndConversation();
            }

            await base.OnDisconnect();
        }
    }
}
