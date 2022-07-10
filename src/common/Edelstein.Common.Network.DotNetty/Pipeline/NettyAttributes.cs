using DotNetty.Common.Utilities;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Network.DotNetty.Pipeline
{
    public class NettyAttributes
    {
        public static readonly AttributeKey<ISocket> SocketKey = AttributeKey<ISocket>.ValueOf("Socket");
        public static readonly AttributeKey<ISession> SessionKey = AttributeKey<ISession>.ValueOf("Session");
    }
}
