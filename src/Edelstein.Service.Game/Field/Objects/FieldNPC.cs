using System.Drawing;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.NPC;

namespace Edelstein.Service.Game.Field.Objects
{
    public class FieldNPC : AbstractFieldControlledLife
    {
        public override FieldObjType Type => FieldObjType.NPC;
        public NPCTemplate Template { get; }

        public int RX0 { get; set; }
        public int RX1 { get; set; }

        public FieldNPC(NPCTemplate template)
            => Template = template;

        public override IPacket GetEnterFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.NpcEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(Template.ID);

                p.Encode<Point>(Position);
                p.Encode<byte>(MoveAction);
                p.Encode<short>(Foothold);

                p.Encode<short>((short) RX0);
                p.Encode<short>((short) RX1);

                p.Encode<bool>(true); // bEnabled
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.NpcLeaveField))
            {
                p.Encode<int>(ID);
                return p;
            }
        }

        public override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using (var p = new Packet(SendPacketOperations.NpcChangeController))
            {
                p.Encode<bool>(setAsController);
                p.Encode<int>(ID);
                return p;
            }
        }

        public async Task OnNpcMove(IPacket packet)
        {
            using (var p = new Packet(SendPacketOperations.NpcMove))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(packet.Decode<byte>()); // TODO: validate acts
                p.Encode<byte>(packet.Decode<byte>());

                if (Template.Move)
                {
                    var path = Move(packet);
                    path.Encode(p);
                }

                await Field.BroadcastPacket(p);
            }
        }
    }
}