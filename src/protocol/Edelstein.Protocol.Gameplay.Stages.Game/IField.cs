using System;
using System.Numerics;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Space;
using Edelstein.Protocol.Network;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IField : IFieldPool, IStage<IField, IFieldObjUser>, IPhysicalSpace2D
    {
        //FieldTemplate Template { get; init; }

        IFieldSplit GetSplit(Vector2 position);
        IFieldSplit[] GetEnclosingSplits(Vector2 position);
        IFieldSplit[] GetEnclosingSplits(IFieldSplit split);

        IFieldPool GetPool(FieldObjType type);
        //IFieldPortal GetPortal(byte portal);
        //IFieldPortal GetPortal(string portal);

        Task Enter(IFieldObjUser user, byte portal, Func<IPacket> getEnterPacket = null);
        Task Enter(IFieldObjUser user, string portal, Func<IPacket> getEnterPacket = null);

        Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null);
        Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null);
    }
}
