using Edelstein.Common.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public class FieldNPCMovePath : AbstractMovePath<IFieldNPCMoveAction>, IFieldNPCMovePath
{
    public byte Act { get; private set; }
    public byte Chat { get; private set; }

    public bool IsMove { get; }
    
    public FieldNPCMovePath(bool isMove) => IsMove = isMove;
    
    public override void ReadFrom(IPacketReader reader)
    {
        Act = reader.ReadByte();
        Chat = reader.ReadByte();

        if (IsMove)
            base.ReadFrom(reader);
    }

    public override void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte(Act);
        writer.WriteByte(Chat);

        if (IsMove)
            base.WriteTo(writer);
    }

    protected override IFieldNPCMoveAction GetActionFromRaw(byte raw) => new FieldNPCMoveAction(raw);
}
