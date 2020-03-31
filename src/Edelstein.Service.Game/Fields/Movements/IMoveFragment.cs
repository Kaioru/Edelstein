using Edelstein.Network.Packets;

namespace Edelstein.Service.Game.Fields.Movements
{
    public interface IMoveFragment
    {
        MoveFragmentAttribute Attribute { get; }

        void Apply(IMoveContext context);
        void Decode(IPacket packet);
        void Encode(IPacket packet);
    }
}