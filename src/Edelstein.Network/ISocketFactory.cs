using DotNetty.Transport.Channels;

namespace Edelstein.Network
{
    public interface ISocketFactory
    {
        ISocket Build(IChannel channel, uint seqSend, uint seqRecv);
    }
}