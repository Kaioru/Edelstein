using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Keys;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class QuickSlotKeyMappedModifiedHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.QuickslotKeyMappedModified;

        protected override Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            for (var i = 0; i < 8; i++)
                user.Character.QuickSlotKeys[i] = new CharacterQuickSlotKey(packet.ReadInt());

            return Task.CompletedTask;
        }
    }
}
