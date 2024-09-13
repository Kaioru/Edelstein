using System;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Templates;

public record FieldTemplateLife : IFieldTemplateLife
{
    public FieldLifeType Type { get; }
    
    public int TemplateID { get; }

    public int MobTime { get; }

    public bool IsFacingLeft { get; }
    
    public IPoint2D Position { get; }
    public IRect2D Bounds { get; }

    public int Foothold { get; }
    
    public FieldTemplateLife(IDataNode node)
    {
        TemplateID = Convert.ToInt32(node.ResolveString("id") ?? "-1");

        Type = node.ResolveString("type")?.ToLower() == "n"
            ? FieldLifeType.NPC
            : FieldLifeType.Monster;

        MobTime = node.ResolveInt("mobTime") ?? 0;

        IsFacingLeft = !(node.ResolveBool("f") ?? false);
        
        Position = new Point2D(
            node.ResolveInt("x") ?? int.MinValue,
            node.ResolveInt("y") ?? int.MinValue
        );
        Bounds = new Rect2D(
            new Point2D(node.ResolveInt("rx0") ?? int.MinValue, Position.Y),
            new Point2D(node.ResolveInt("rx1") ?? int.MaxValue, Position.Y)
        );
        
        Foothold = node.ResolveInt("fh") ?? 0;
    }
}
