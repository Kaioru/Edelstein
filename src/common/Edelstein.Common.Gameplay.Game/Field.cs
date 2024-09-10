using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game;

public class Field : AbstractFieldObjectPool, IField
{
    private const int ScreenWidth = 1024;
    private const int ScreenHeight = 768;
    private const int ScreenWidthOffset = ScreenWidth * 75 / 100;
    private const int ScreenHeightOffset = ScreenHeight * 75 / 100;

    private readonly Dictionary<FieldObjectType, FieldObjectPool> _pools;
    private readonly IFieldSplit[,] _splits;
    
    private readonly SemaphoreSlim _lock = new(1, 1);

    public int ID => Template.ID;
    public IFieldTemplate Template { get; }
    
    public Field(IFieldTemplate template)
    {
        Template = template;

        _pools = new Dictionary<FieldObjectType, FieldObjectPool>();
        foreach (var type in Enum.GetValues<FieldObjectType>())
            _pools[type] = new FieldObjectPool();

        var splitRowCount = (int)(template.Bounds.Height + (ScreenHeightOffset - 1)) / ScreenHeightOffset;
        var splitColCount = (int)(template.Bounds.Width + (ScreenWidthOffset - 1)) / ScreenWidthOffset;

        _splits = new IFieldSplit[splitRowCount, splitColCount];

        for (var row = 0; row < splitRowCount; row++)
        for (var col = 0; col < splitColCount; col++)
            _splits[row, col] = new FieldSplit(row, col);
    }

    public override IFieldObject? GetObject(int id)
        => GetObjects().FirstOrDefault(o => o.ObjectID == id);
    
    public override IEnumerable<IFieldObject> GetObjects()
        => _pools.Values.SelectMany(p => p.GetObjects()).ToImmutableList();
    
    private IFieldSplit? GetSplit(int row, int col)
    {
        if (
            row < 0 || row >= _splits.GetLength(0) ||
            col < 0 || col >= _splits.GetLength(1)
        ) return null;
        return _splits[row, col];
    }
    
    public IFieldSplit? GetSplit(IPoint2D position)
    {
        var row = (position.Y - Template.Bounds.Top) / ScreenHeightOffset;
        var col = (position.X - Template.Bounds.Left) / ScreenWidthOffset;
        return GetSplit(row, col);
    }
    
    public IFieldSplit?[] GetSplits(IRect2D bounds)
    {
        var minRow = (bounds.Top - Template.Bounds.Top) / ScreenHeightOffset;
        var maxRow = (bounds.Bottom - Template.Bounds.Top) / ScreenHeightOffset;
        var minCol = (bounds.Left - Template.Bounds.Left) / ScreenWidthOffset;
        var maxCol = (bounds.Right - Template.Bounds.Left) / ScreenWidthOffset;
        var splits = new IFieldSplit?[(maxRow - minRow + 1) * (maxCol - minCol + 1)];
        var index = 0;
        
        for (var row = minRow; row <= maxRow; row++)
        for (var col = minCol; col <= maxCol; col++)
            splits[index++] = GetSplit(row, col);
        return splits;
    }

    private IFieldSplit?[] GetEnclosingSplits(int row, int col)
    {
        var splits = new IFieldSplit?[9];

        splits[0] = GetSplit(row - 1, col - 1);
        splits[1] = GetSplit(row - 1, col);
        splits[2] = GetSplit(row - 1, col + 1);

        splits[3] = GetSplit(row, col - 1);
        splits[4] = GetSplit(row, col);
        splits[5] = GetSplit(row, col + 1);

        splits[6] = GetSplit(row + 1, col - 1);
        splits[7] = GetSplit(row + 1, col);
        splits[8] = GetSplit(row + 1, col + 1);

        return splits;
    }
    
    public IFieldSplit?[] GetEnclosingSplits(IPoint2D position)
    {
        var row = (position.Y - Template.Bounds.Top) / ScreenHeightOffset;
        var col = (position.X - Template.Bounds.Left) / ScreenWidthOffset;
        return GetEnclosingSplits(row, col);
    }

    public IFieldSplit?[] GetEnclosingSplits(IFieldSplit split) =>
        GetEnclosingSplits(split.Row, split.Col);
    
    public IFieldObjectPool? GetPool(FieldObjectType type)
        => _pools.TryGetValue(type, out var pool) ? pool : null;
    
    public override async Task Enter(IFieldObject obj)
    {
        await _lock.WaitAsync();

        try
        {
            var pool = GetPool(obj.Type);

            if (obj.Field != null)
                await obj.Field.Leave(obj);
            obj.Field = this;


            if (obj is IFieldUser user)
            {
                await user.Dispatch(user.GetDispatchSetField());
            }

            var split = GetSplit(obj.Position);

            if (pool != null) await pool.Enter(obj);
            if (split != null) await split.Enter(obj);
        }
        finally
        {
            _lock.Release();
        }
    }
    
    public override async Task Leave(IFieldObject obj)
    {
        await _lock.WaitAsync();

        try
        {
            var pool = GetPool(obj.Type);

            obj.Field = null;

            if (obj.FieldSplit != null)
            {
                if (obj is IFieldUser observer)
                    foreach (var split in observer.Observing.ToImmutableArray())
                        await split.Unobserve(observer, true);
                await obj.FieldSplit.Leave(obj);
            }
            if (pool != null) await pool.Leave(obj);
        }
        finally
        {
            _lock.Release();
        }
    }
}
