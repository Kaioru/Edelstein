using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Movements
{
    public interface IMovePath
    {
        MoveActionType? Type { get; }
        Point2D? Position { get; }
        short? Foothold { get; }

        void Decode(IPacketReader reader);
        void Encode(IPacketWriter writer);
    }
}
