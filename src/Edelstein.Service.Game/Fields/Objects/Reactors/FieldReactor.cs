using System.Drawing;
using Edelstein.Core;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Reactor;
using Edelstein.Service.Game.Fields.Generators;

namespace Edelstein.Service.Game.Fields.Objects.Reactors
{
    public class FieldReactor : AbstractFieldObj, IFieldGeneratedObj
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
            using (var p = new Packet(SendPacketOperations.ReactorEnterField))
            {
                p.Encode<int>(ID);
                p.Encode<int>(Template.ID);
                p.Encode<byte>(1); // State
                p.Encode<Point>(Position);
                p.Encode<bool>(Flip);
                p.Encode<string>(""); // Name?
                return p;
            }
        }

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.ReactorLeaveField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(1); // State
                p.Encode<Point>(Position);
                return p;
            }
        }
    }
}