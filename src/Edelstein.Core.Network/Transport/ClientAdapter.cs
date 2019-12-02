using System;
using DotNetty.Transport.Channels;
using Edelstein.Network.Logging;
using Edelstein.Network.Packets;

namespace Edelstein.Network.Transport
{
    public class ClientAdapter : ChannelHandlerAdapter
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        private readonly Client _client;

        public ClientAdapter(Client client)
            => _client = client;

        public override void ChannelRead(IChannelHandlerContext context, object message)
        {
            var socket = context.Channel.GetAttribute(Socket.Key).Get();
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();

            var p = (IPacket) message;

            if (adapter != null) adapter.OnPacket(p);
            else
            {
                var version = p.Decode<short>();
                var patch = p.Decode<string>();

                var seqSend = p.Decode<uint>();
                var seqRecv = p.Decode<uint>();

                var locale = p.Decode<byte>();

                if (version != _client.Version) return;
                if (patch != _client.Patch) return;
                if (locale != _client.Locale) return;

                var newSocket = new Socket(
                    context.Channel,
                    seqSend,
                    seqRecv
                );
                var newAdapter = _client.AdapterFactory.Build(newSocket);
                
                lock (_client)
                {
                    if (_client.Socket == null)
                        _client.Socket = newSocket;
                }

                context.Channel.GetAttribute(Socket.Key).Set(newSocket);
                context.Channel.GetAttribute(AbstractSocketAdapter.Key).Set(newAdapter);
                Logger.Debug($"Initialized connection to {context.Channel.RemoteAddress}");
            }
        }

        public override void ChannelInactive(IChannelHandlerContext context)
        {
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();

            adapter?.OnDisconnect();
            base.ChannelInactive(context);

            Logger.Debug($"Released connection to {context.Channel.RemoteAddress}");
        }


        public override void ExceptionCaught(IChannelHandlerContext context, Exception exception)
        {
            var adapter = context.Channel.GetAttribute(AbstractSocketAdapter.Key).Get();
            adapter?.OnException(exception);
        }
    }
}