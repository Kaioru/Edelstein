using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IFieldInfo
    {
        public FieldOpt Limit { get; }
        public Rect2D Bounds { get; }

        public int? FieldReturn { get; }
        public int? ForcedReturn { get; }

        public string ScriptFirstUserEnter { get; }
        public string ScriptUserEnter { get; }

        public double MobRate { get; }
        public int MobCapacityMin { get; }
        public int MobCapacityMax { get; }
    }
}
