using Edelstein.Common.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.NPC.Movements;

public class NPCMovePath : AbstractMovePath<INPCMoveAction>, INPCMovePath
{
    public NPCMovePath(bool isMove) => IsMove = isMove;

    public byte Act { get; private set; }
    public byte Chat { get; private set; }
    public int Duration { get; private set; }

    public bool IsMove { get; }

    public override void ReadFrom(IPacketReader reader)
    {
        Act = reader.ReadByte();
        Chat = reader.ReadByte();
        Duration = reader.ReadInt();

        if (IsMove)
            base.ReadFrom(reader);
    }

    public override void WriteTo(IPacketWriter writer)
    {
        writer.WriteByte(Act);
        writer.WriteByte(Chat);
        writer.WriteInt(Duration);

        if (IsMove)
            base.WriteTo(writer);
    }

    protected override INPCMoveAction GetActionFromRaw(byte raw) => new NPCMoveAction(raw);
}
