using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates;

public interface IFieldTemplate : ITemplate
{
    FieldLimitType Limit { get; }

    IRectangle2D Bounds { get; }
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
