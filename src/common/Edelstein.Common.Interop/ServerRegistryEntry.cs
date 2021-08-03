using System;
using System.Threading.Channels;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class ServerRegistryEntry : IDisposable
    {
        public ServerObject Server { get; }
        public Channel<DispatchObject> Dispatch { get; }

        public ServerRegistryEntry(ServerObject server)
        {
            Server = server;
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
