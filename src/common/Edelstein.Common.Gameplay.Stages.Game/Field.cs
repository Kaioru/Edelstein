using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public class Field : IField
    {
        public int ID => Template.ID;
        public Rect2D Bounds => Template.Bounds;

        public FieldTemplate Template { get; }

        public IPhysicalPoint2D GetPortal(int id) { throw new NotImplementedException(); }
        public IEnumerable<IPhysicalPoint2D> GetPortals() { throw new NotImplementedException(); }

        public IPhysicalPoint2D GetStartPoint(int id) { throw new NotImplementedException(); }
        public IPhysicalPoint2D GetStartPointClosestTo(Point2D point) { throw new NotImplementedException(); }
        public IEnumerable<IPhysicalPoint2D> GetStartPoints() { throw new NotImplementedException(); }

        public IPhysicalLine2D GetFoothold(int id) { throw new NotImplementedException(); }
        public IPhysicalLine2D GetFootholdClosestTo(Point2D point) { throw new NotImplementedException(); }
        public IPhysicalLine2D GetFootholdUnderneath(Point2D point) { throw new NotImplementedException(); }
        public IEnumerable<IPhysicalLine2D> GetFootholds() { throw new NotImplementedException(); }

        public IPhysicalLine2D GetLadderOrRope(int id) { throw new NotImplementedException(); }
        public IEnumerable<IPhysicalLine2D> GetLadderOrRopes() { throw new NotImplementedException(); }

        public IFieldSplit GetSplit(Point2D position) { throw new NotImplementedException(); }
        public IFieldSplit[] GetEnclosingSplits(Point2D position) { throw new NotImplementedException(); }
        public IFieldSplit[] GetEnclosingSplits(IFieldSplit split) { throw new NotImplementedException(); }

        public IFieldPool GetPool(FieldObjType type) { throw new NotImplementedException(); }

        public Task Enter(IFieldObjUser user) { throw new NotImplementedException(); }
        public Task Leave(IFieldObjUser user) { throw new NotImplementedException(); }

        public Task Enter(IFieldObjUser user, byte portal, Func<IPacket> getEnterPacket = null) { throw new NotImplementedException(); }
        public Task Enter(IFieldObjUser user, string portal, Func<IPacket> getEnterPacket = null) { throw new NotImplementedException(); }

        public Task Enter(IFieldObj obj) { throw new NotImplementedException(); }
        public Task Leave(IFieldObj obj) { throw new NotImplementedException(); }

        public Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null) { throw new NotImplementedException(); }
        public Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null) { throw new NotImplementedException(); }

        public IFieldObjUser GetUser(int id) { throw new NotImplementedException(); }
        public IEnumerable<IFieldObjUser> GetUsers() { throw new NotImplementedException(); }

        public IFieldObj GetObject(int id) { throw new NotImplementedException(); }
        public T GetObject<T>(int id) where T : IFieldObj { throw new NotImplementedException(); }

        public IEnumerable<IFieldObj> GetObjects() { throw new NotImplementedException(); }
        public IEnumerable<T> GetObjects<T>() where T : IFieldObj { throw new NotImplementedException(); }

        public Task Dispatch(IFieldObj source, IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Where(u => u.ID != source.ID).Select(u => u.Dispatch(packet)));

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Select(u => u.Dispatch(packet)));
    }
}
