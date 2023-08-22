using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Spatial;

public record FieldPortal : IFieldPortal
{
    public int ID { get; }

    public int MinX => Position.MinX;
    public int MinY => Position.MinY;

    public int MaxX => Position.MaxX;
    public int MaxY => Position.MaxY;

    public FieldPortalType Type { get; }

    public string Name { get; }
    public string? Script { get; }

    public int ToMap { get; }
    public string? ToName { get; }

    public IPoint2D Position { get; }
    
    public FieldPortal(int id, IDataProperty property)
    {
        ID = id;

        Name = property.ResolveOrDefault<string>("pn")!;
        Type = (FieldPortalType)(property.Resolve<int>("pt") ?? 0);
        Script = property.ResolveOrDefault<string>("script");
        ToMap = property.Resolve<int>("tm") ?? int.MinValue;
        ToName = property.ResolveOrDefault<string>("tn");
        Position = new Point2D(
            property.Resolve<int>("x") ?? int.MinValue,
            property.Resolve<int>("y") ?? int.MinValue
        );
    }

}
