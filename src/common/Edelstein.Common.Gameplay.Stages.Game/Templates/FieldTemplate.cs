using System.Collections.Immutable;
using Edelstein.Common.Util.Spatial;
using Edelstein.Common.Util.Spatial.Collections;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates;

public record FieldTemplate : IFieldTemplate
{
    public FieldTemplate(
        int id,
        IDataNode foothold,
        IDataProperty portal,
        IDataProperty ladderRope,
        IDataProperty life,
        IDataProperty info
    )
    {
        ID = id;

        Limit = (FieldLimitType)(info.Resolve<int>("fieldLimit") ?? 0);

        FieldReturn = info.Resolve<int>("returnMap");
        ForcedReturn = info.Resolve<int>("forcedReturn");
        if (FieldReturn == 999999999) FieldReturn = null;
        if (ForcedReturn == 999999999) ForcedReturn = null;

        ScriptFirstUserEnter = info.ResolveOrDefault<string>("onFirstUserEnter");
        ScriptUserEnter = info.ResolveOrDefault<string>("onUserEnter");
        if (string.IsNullOrWhiteSpace(ScriptFirstUserEnter)) ScriptFirstUserEnter = null;
        if (string.IsNullOrWhiteSpace(ScriptUserEnter)) ScriptUserEnter = null;

        var footholds = foothold.Children
            .SelectMany(c => c.Children)
            .SelectMany(c => c.Children)
            .Select(p => new FieldTemplateFoothold(Convert.ToInt32(p.Name), p.ResolveAll()))
            .ToImmutableList();

        var leftTop = new Point2D(
            footholds.Min(f => f.MinX),
            footholds.Min(f => f.MinY)
        );
        var rightBottom = new Point2D(
            footholds.Max(f => f.MaxX),
            footholds.Max(f => f.MaxY)
        );

        leftTop = new Point2D(
            info.Resolve<int>("VRLeft") ?? leftTop.X,
            info.Resolve<int>("VRTop") ?? leftTop.Y
        );
        rightBottom = new Point2D(
            info.Resolve<int>("VRRight") ?? rightBottom.X,
            info.Resolve<int>("VRBottom") ?? rightBottom.Y
        );

        Bounds = new Rectangle2D(leftTop, rightBottom);

        Footholds = new FieldSpace<IFieldTemplateFoothold>(Bounds);
        Footholds.BulkInsert(footholds);

        MobRate = info.Resolve<double>("mobRate") ?? 1.0;

        var mobCapacity = Bounds.Height * Bounds.Width * MobRate * 0.0000078125;

        mobCapacity = Math.Min(mobCapacity, 40);
        mobCapacity = Math.Max(mobCapacity, 1);

        MobCapacityMin = (int)mobCapacity;
        MobCapacityMax = (int)mobCapacity * 2;
    }

    public int ID { get; }

    public FieldLimitType Limit { get; }

    public IRectangle2D Bounds { get; }

    public IFieldSpace<IFieldTemplateFoothold> Footholds { get; }

    public int? FieldReturn { get; }
    public int? ForcedReturn { get; }

    public string? ScriptFirstUserEnter { get; }
    public string? ScriptUserEnter { get; }

    public double MobRate { get; }
    public int MobCapacityMin { get; }
    public int MobCapacityMax { get; }
}
