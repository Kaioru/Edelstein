﻿using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Movements.Fragments;

public class JumpFragment : ActionFragment
{
    private IPoint2D _vPosition;

    public JumpFragment(MovePathFragmentType type) : base(type)
    {
    }

    protected override void ReadBody(IPacketReader reader)
    {
        _vPosition = reader.ReadPoint2D();

        base.ReadBody(reader);
    }

    protected override void WriteBody(IPacketWriter writer)
    {
        writer.WritePoint2D(_vPosition);

        base.WriteBody(writer);
    }
}
