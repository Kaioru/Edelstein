using System;
using System.Linq;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Util.Spatial;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public class FieldMobNormalGenerator : AbstractFieldMobGenerator
    {
        private DateTime LastRegenCheck { get; set; }

        public FieldMobNormalGenerator(
            FieldLifeTemplate lifeTemplate,
            MobTemplate mobTemplate
        ) : base(lifeTemplate, mobTemplate)
        {
            LastRegenCheck = DateTime.UtcNow;
        }

        public override bool Check(DateTime now, IField field)
        {
            if ((now - LastRegenCheck).TotalSeconds >= 7) return false;

            var bounds = new Rect2D(
                new Point2D(
                    LifeTemplate.Position.X - 100,
                    LifeTemplate.Position.Y - 100
                ),
                new Size2D(200, 200)
            );

            LastRegenCheck = DateTime.UtcNow;

            return !field
                    .GetObjects<IFieldObjMob>()
                    .Any(m => bounds.Contains(m.Position));
        }
    }
}
