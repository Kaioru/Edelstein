using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements
{
    public interface IMoveFragment
    {
        void Apply(IFieldLife life);
        void Decode(IPacket packet);
        void Encode(IPacket packet);
    }
}