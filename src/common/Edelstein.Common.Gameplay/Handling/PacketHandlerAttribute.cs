using System;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Handling;

[AttributeUsage(AttributeTargets.Class)]
public class PacketHandlerAttribute(
    PacketRecvOperation operation
) : Attribute
{
    public PacketRecvOperation Operation { get; } = operation;
}
