using System.IO;

namespace Edelstein.Protocol.Network.Packets;

public interface IDispatchable
{
    void DispatchTo(Stream output);
}
