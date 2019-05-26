using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Interactions.Miniroom
{
    public class MiniroomDialog : AbstractDialog, IMiniroomDialog
    {
        public IMiniroom Miniroom { get; }

        public MiniroomDialog(FieldUser user, IMiniroom miniroom) : base(user)
        {
            Miniroom = miniroom;
        }

        public override async Task Enter()
        {
            var result = await Miniroom.Enter(User);
            if (result != MiniroomEnterResult.Success)
            {
                await User.Interact(close: true);
                using (var p = new Packet(SendPacketOperations.MiniRoom))
                {
                    p.Encode<byte>((byte) MiniroomAction.MRP_EnterResult);
                    p.Encode<byte>(0x0);
                    p.Encode<byte>((byte) result);
                    await User.SendPacket(p);
                }
            }
        }

        public override async Task Leave()
        {
            await Miniroom.Leave(User);
            await User.Interact(close: true);
        }
    }
}