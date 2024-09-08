using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Templates;

public interface IFieldTemplate : ITemplate
{
    FieldLimitType Limit { get; }

    IRect2D Bounds { get; }
    IFieldSpace<IFieldFoothold> Footholds { get; }
    IFieldSpace<IFieldPortal> StartPoints { get; }
    IFieldSpace<IFieldPortal> Portals { get; }

    int? FieldReturn { get; }
    int? ForcedReturn { get; }

    string? ScriptFirstUserEnter { get; }
    string? ScriptUserEnter { get; }

    double MobRate { get; }
    int MobCapacityMin { get; }
    int MobCapacityMax { get; }
}
