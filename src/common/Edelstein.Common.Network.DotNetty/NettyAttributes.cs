using DotNetty.Common.Utilities;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Network.DotNetty;

public static class NettyAttributes
{
    public static readonly AttributeKey<ISocket> SocketKey = AttributeKey<ISocket>.ValueOf("Socket");
    public static readonly AttributeKey<IAdapter> AdapterKey = AttributeKey<IAdapter>.ValueOf("Adapter");
}
