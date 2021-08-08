using System;
using System.Collections.Generic;
using System.Collections.Immutable;
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
        private const int ScreenWidth = 1024;
        private const int ScreenHeight = 768;
        private const int ScreenWidthOffset = ScreenWidth * 75 / 100;
        private const int ScreenHeightOffset = ScreenHeight * 75 / 100;

        public int ID => Template.ID;
        public Rect2D Bounds => Template.Bounds;

        public GameStage Stage { get; }
        public FieldTemplate Template { get; }

        private readonly object _objectLock;
        private readonly IDictionary<FieldObjType, IFieldPool> _pools;
        private readonly IFieldSplit[,] _splits;

        // TODO: Better physicalspace2d handling
        public Field(GameStage stage, FieldTemplate template)
        {
            Stage = stage;
            Template = template;

            _objectLock = new object();
            _pools = new Dictionary<FieldObjType, IFieldPool>();

            var splitColCount = (template.Bounds.Size.Width + (ScreenWidthOffset - 1)) / ScreenWidthOffset;
            var splitRowCount = (template.Bounds.Size.Height + (ScreenHeightOffset - 1)) / ScreenHeightOffset;

            _splits = new IFieldSplit[splitColCount, splitRowCount];

            for (var row = 0; row < splitRowCount; row++)
                for (var col = 0; col < splitColCount; col++)
                    _splits[row, col] = new FieldSplit(row, col);
        }

        public IPhysicalPoint2D GetPortal(int id)
        {
            if (Template.Portals.ContainsKey(id))
                return Template.Portals[id];
            return null;
        }

        public IEnumerable<IPhysicalPoint2D> GetPortals() => Template.Portals.Values.ToImmutableList();

        public IPhysicalPoint2D GetStartPoint(int id)
        {
            if (Template.Portals
                    .Where(kv => kv.Value.Type == FieldPortalType.StartPoint)
                    .ToImmutableDictionary()
                    .ContainsKey(id))
                return Template.Portals[id];
            return null;
        }

        public IPhysicalPoint2D GetStartPointClosestTo(Point2D point)
            => GetStartPoints().OrderBy(p => p.Position.Distance(point)).FirstOrDefault();

        public IEnumerable<IPhysicalPoint2D> GetStartPoints()
            => Template.Portals.Values.Where(p => p.Type == FieldPortalType.StartPoint).ToImmutableList();

        public IPhysicalLine2D GetFoothold(int id)
        {
            if (Template.Footholds.ContainsKey(id))
                return Template.Footholds[id];
            return null;
        }

        public IPhysicalLine2D GetFootholdClosestTo(Point2D point)
            => GetFootholds()
                .OrderBy(f => f.Line.Start.Distance(point) + f.Line.End.Distance(point))
                .FirstOrDefault();

        public IPhysicalLine2D GetFootholdUnderneath(Point2D point)
            => GetFootholdClosestTo(point);

        public IEnumerable<IPhysicalLine2D> GetFootholds() => Template.Footholds.Values.ToImmutableList();

        public IPhysicalLine2D GetLadderOrRope(int id)
        {
            if (Template.LadderOrRopes.ContainsKey(id))
                return Template.LadderOrRopes[id];
            return null;
        }

        public IEnumerable<IPhysicalLine2D> GetLadderOrRopes() => Template.LadderOrRopes.Values.ToImmutableList();

        private IFieldSplit GetSplit(int row, int col)
        {
            if (
                row < 0 || row >= _splits.GetLength(0) ||
                col < 0 || col >= _splits.GetLength(1)
            ) return null;
            return _splits[row, col];
        }

        public IFieldSplit GetSplit(Point2D position)
        {
            var row = (position.Y - Template.Bounds.Top) / ScreenHeightOffset;
            var col = (position.X - Template.Bounds.Left) / ScreenWidthOffset;

            return GetSplit(row, col);
        }

        public IFieldSplit[] GetEnclosingSplits(Point2D position)
            => GetEnclosingSplits(GetSplit(position));

        public IFieldSplit[] GetEnclosingSplits(IFieldSplit split)
        {
            var splits = new IFieldSplit[9];

            if (split == null) return splits;

            var row = split.Row;
            var col = split.Col;

            splits[0] = GetSplit(row - 1, col - 1);
            splits[1] = GetSplit(row - 1, col);
            splits[2] = GetSplit(row - 1, col + 1);

            splits[3] = GetSplit(row, col - 1);
            splits[4] = split;
            splits[5] = GetSplit(row, col + 1);

            splits[6] = GetSplit(row + 1, col - 1);
            splits[7] = GetSplit(row + 1, col);
            splits[8] = GetSplit(row + 1, col + 1);

            return splits;
        }

        public IFieldPool GetPool(FieldObjType type)
        {
            lock (_objectLock)
            {
                _pools.TryGetValue(type, out var pool);

                if (pool == null)
                {
                    pool = new FieldPool();
                    _pools[type] = pool;
                }

                return pool;
            }
        }

        public Task Enter(IFieldObjUser user) => Enter(user, 0, null);
        public Task Leave(IFieldObjUser user) => Leave(user, null);

        public Task Enter(IFieldObjUser user, byte portal, Func<IPacket> getEnterPacket = null)
        {
            user.Character.FieldPortal = portal;
            return Enter(user, getEnterPacket);
        }

        public Task Enter(IFieldObjUser user, string portal, Func<IPacket> getEnterPacket = null)
        {
            user.Character.FieldPortal = (byte)Template.Portals
                .FirstOrDefault(kv => kv.Value.Name.Equals(portal))
                .Key;
            return Enter(user, getEnterPacket);
        }

        public Task Enter(IFieldObj obj) => Enter(obj, null);
        public Task Leave(IFieldObj obj) => Leave(obj, null);

        public async Task Enter(IFieldObj obj, Func<IPacket> getEnterPacket = null) {
            var pool = GetPool(obj.Type);

            obj.Field?.Leave(obj);
            obj.Field = this;

            if (obj is IFieldObjUser user)
            {
                var portal = GetPortal(user.Character.FieldPortal) ?? GetStartPointClosestTo(user.Position);

                user.Character.FieldID = Template.ID;
                user.Position = portal.Position;
                user.Foothold = (short)GetFootholdUnderneath(portal.Position).ID;

                await user.Dispatch(user.GetSetFieldPacket());

                if (!user.IsInstantiated) user.IsInstantiated = true;
            }

            await pool.Enter(obj);
            await obj.UpdateFieldSplit(getEnterPacket);
        }

        public async Task Leave(IFieldObj obj, Func<IPacket> getLeavePacket = null) {
            var pool = GetPool(obj.Type);

            obj.Field = null;

            if (obj is IFieldObjUser user)
                user.Watching.Clear();

            await pool.Leave(obj);
            await obj.UpdateFieldSplit(getLeavePacket: getLeavePacket);
        }

        public IFieldObjUser GetUser(int id) => GetObjects<IFieldObjUser>().FirstOrDefault(u => u.ID == id);
        public IEnumerable<IFieldObjUser> GetUsers() => GetObjects<IFieldObjUser>();

        public IFieldObj GetObject(int id) => GetObjects().FirstOrDefault(o => o.ID == id);
        public T GetObject<T>(int id) where T : IFieldObj => GetObjects<T>().FirstOrDefault(o => o.ID == id);

        public IEnumerable<IFieldObj> GetObjects() => _pools.Values.SelectMany(p => p.GetObjects()).ToImmutableList();
        public IEnumerable<T> GetObjects<T>() where T : IFieldObj => _pools.Values.SelectMany(p => p.GetObjects<T>()).ToImmutableList();

        public Task Dispatch(IFieldObj source, IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Where(u => u.ID != source.ID).Select(u => u.Dispatch(packet)));

        public Task Dispatch(IPacket packet)
            => Task.WhenAll(GetObjects<IFieldObjUser>().Select(u => u.Dispatch(packet)));
    }
}
