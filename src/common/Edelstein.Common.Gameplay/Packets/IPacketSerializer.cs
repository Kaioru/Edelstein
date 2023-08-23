﻿using Edelstein.Protocol.Gameplay;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Packets;

public interface IPacketSerializer<in TStageUser, out TObject>
    where TStageUser : IStageUser<TStageUser>
{
    TObject? Serialize(TStageUser user, IPacketReader reader);
}
