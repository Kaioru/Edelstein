using Edelstein.Common.Util.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Templates;

public record FieldTemplateLife : IFieldTemplateLife
{
    public FieldTemplateLife(IDataProperty property)
    {
        ID = property.Resolve<int>("id") ?? -1;

        Type = property.ResolveOrDefault<string>("type")?.ToLower() == "n"
            ? FieldLifeType.NPC
            : FieldLifeType.Monster;

        MobTime = property.Resolve<int>("mobTime") ?? 0;

        IsFacingLeft = !(property.Resolve<bool>("f") ?? false);
        Position = new Point2D(
            property.Resolve<int>("x") ?? int.MinValue,
            property.Resolve<int>("y") ?? int.MinValue
        );
        Bounds = new Rectangle2D(
            new Point2D(property.Resolve<int>("rx0") ?? int.MinValue, Position.Y),
            new Point2D(property.Resolve<int>("rx1") ?? int.MaxValue, Position.Y)
        );
        FootholdID = property.Resolve<int>("fh") ?? 0;
    }

    public int ID { get; }

    public FieldLifeType Type { get; }

    public int MobTime { get; }

    public bool IsFacingLeft { get; }
    public IPoint2D Position { get; }
    public IRectangle2D Bounds { get; }
    public int FootholdID { get; }
}
