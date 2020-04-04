using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields.Movements
{
    public interface IMovePath
    {
        IMoveContext Apply(IFieldLife life);
        void Decode(IPacketDecoder packet);
        void Encode(IPacketEncoder packet);
    }
}