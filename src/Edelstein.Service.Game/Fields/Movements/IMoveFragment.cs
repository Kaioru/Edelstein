using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.Movements
{
    public interface IMoveFragment
    {
        void Apply(IFieldLife life);
        void Decode(IPacket packet);
        void Encode(IPacket packet);
    }
}