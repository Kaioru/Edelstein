using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Templates;

public record FieldTemplateLife : IFieldTemplateLife
{

    public FieldTemplateLife(IDataNode node)
    {
        ID = node.ResolveInt("id") ?? -1;

        Type = node.ResolveString("type")?.ToLower() == "n"
            ? FieldLifeType.NPC
            : FieldLifeType.Monster;

        MobTime = node.ResolveInt("mobTime") ?? 0;

        IsFacingLeft = !(node.ResolveBool("f") ?? false);
        Position = new Point2D(
            node.ResolveInt("x") ?? int.MinValue,
            node.ResolveInt("y") ?? int.MinValue
        );
        Bounds = new Rectangle2D(
            new Point2D(node.ResolveInt("rx0") ?? int.MinValue, Position.Y),
            new Point2D(node.ResolveInt("rx1") ?? int.MaxValue, Position.Y)
        );
        FootholdID = node.ResolveInt("fh") ?? 0;
    }
    public int ID { get; }

    public FieldLifeType Type { get; }

    public int MobTime { get; }

    public bool IsFacingLeft { get; }
    public IPoint2D Position { get; }
    public IRectangle2D Bounds { get; }

    public int FootholdID { get; }
}
