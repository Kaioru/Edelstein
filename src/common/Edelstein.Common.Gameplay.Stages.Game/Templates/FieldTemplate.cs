using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Game.Spatial;
using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates;

public record FieldTemplate : IFieldTemplate
{
    public FieldTemplate(
        int id,
        IDataNode foothold,
        IDataNode portal,
        IDataProperty ladderRope,
        IDataNode life,
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
            .Select(p => new FieldFoothold(Convert.ToInt32(p.Name), p.ResolveAll()))
            .ToImmutableList();
        var portals = portal.Children
            .Select(p => new FieldPortal(Convert.ToInt32(p.Name), p.ResolveAll()))
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

        Footholds = new FieldSpace<IFieldFoothold>(Bounds);
        Footholds.BulkInsert(footholds);

        StartPoints = new FieldSpace<IFieldPortal>(Bounds);
        StartPoints.BulkInsert(portals.Where(p => p.Type == FieldPortalType.StartPoint));

        Portals = new FieldSpace<IFieldPortal>(Bounds);
        Portals.BulkInsert(portals);

        Life = life.Children
            .Select(p => new FieldTemplateLife(p.ResolveAll()))
            .ToImmutableList<IFieldTemplateLife>();

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

    public IFieldSpace<IFieldFoothold> Footholds { get; }
    public IFieldSpace<IFieldPortal> StartPoints { get; }
    public IFieldSpace<IFieldPortal> Portals { get; }

    public int? FieldReturn { get; }
    public int? ForcedReturn { get; }

    public string? ScriptFirstUserEnter { get; }
    public string? ScriptUserEnter { get; }

    public ICollection<IFieldTemplateLife> Life { get; }

    public double MobRate { get; }
    public int MobCapacityMin { get; }
    public int MobCapacityMax { get; }
}
