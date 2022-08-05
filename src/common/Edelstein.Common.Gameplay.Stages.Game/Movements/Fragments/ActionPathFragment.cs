﻿using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Util.Buffers.Packets;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class ActionPathFragment<TMoveAction> : AbstractMovePathFragment<TMoveAction>
    where TMoveAction : IMoveAction
{
    private byte _action;
    private short _elapse;

    public ActionPathFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _action = reader.ReadByte();
        _elapse = reader.ReadShort();
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WriteByte(_action);
        writer.WriteShort(_elapse);
    }

    public override void Apply(AbstractMovePath<TMoveAction> path) =>
        path.ActionRaw = _action;
}
