using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class GameStageUser : AbstractMigrateableStageUser<GameStage, GameStageUser, GameStageConfig>, IGameStageUser<GameStage, GameStageUser>
    {
        public IField Field { get; set; }
        public IFieldObjUser FieldUser { get; set; }

        public GameStageUser(ISocket socket) : base(socket) { }
    }
}
