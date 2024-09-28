using System;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Templates;

public record FieldTemplateReactor : IFieldTemplateReactor
{
    public string? Name { get; }
    public int TemplateID { get; }
    
    public int ReactorTime { get; }
    
    public bool Flip { get; }
    public IPoint2D Position { get; }
    
    public FieldTemplateReactor(IDataNode node)
    {
        TemplateID = Convert.ToInt32(node.ResolveString("id") ?? "-1");

        ReactorTime = node.ResolveInt("reactorTime") ?? 0;

        Flip = node.ResolveBool("f") ?? false;
        
        Position = new Point2D(
            node.ResolveInt("x") ?? int.MinValue,
            node.ResolveInt("y") ?? int.MinValue
        );
        
        Name = node.ResolveString("name") ?? "NO-NAME";
    }
}
