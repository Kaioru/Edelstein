﻿using System;
using System.Threading.Channels;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class ServerRegistryEntry : IDisposable
    {
        public ServerObject Server { get; set; }
        public DateTime LastUpdate { get; set; }
        public Channel<DispatchObject> Dispatch { get; }

        public ServerRegistryEntry(ServerObject server, DateTime lastUpdate)
        {
            Server = server;
            LastUpdate = lastUpdate;
            Dispatch = Channel.CreateBounded<DispatchObject>(new BoundedChannelOptions(1)
            {
                FullMode = BoundedChannelFullMode.DropOldest,
                SingleWriter = true
            });
        }

        public void Dispose()
        {
            Dispatch.Writer.Complete();
        }
    }
}
