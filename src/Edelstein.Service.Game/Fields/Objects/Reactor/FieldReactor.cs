using Edelstein.Core.Templates.Reactor;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Generators;

namespace Edelstein.Service.Game.Fields.Objects.Reactor
{
    public class FieldReactor : AbstractFieldObj, IFieldGeneratorObj
    {
        public override FieldObjType Type => FieldObjType.Reactor;
        public ReactorTemplate Template { get; }
        public bool Flip { get; }
        public IFieldGenerator Generator { get; set; }

        public FieldReactor(ReactorTemplate template, bool flip)
        {
            Template = template;
            Flip = flip;
        }

        public override IPacket GetEnterFieldPacket()
        {
            using var p = new OutPacket(SendPacketOperations.ReactorEnterField);
            p.EncodeInt(ID);
            p.EncodeInt(Template.ID);
            p.EncodeByte(0); // State
            p.EncodePoint(Position);
            p.EncodeBool(Flip);
            p.EncodeString(""); // Name?
            return p;
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using var p = new OutPacket(SendPacketOperations.ReactorLeaveField);
            p.EncodeInt(ID);
            p.EncodeByte(0); // State
            p.EncodePoint(Position);
            return p;
        }
    }
}