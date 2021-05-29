﻿using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Network
{
    public interface IPacketDispatcher
    {
        Task Dispatch(IPacket packet);
        Task Dispatch(IEnumerable<IPacket> packets);
    }
}