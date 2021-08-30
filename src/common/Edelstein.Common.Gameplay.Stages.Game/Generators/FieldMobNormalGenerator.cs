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
        private DateTime NextRegenCheck { get; set; }

        public FieldMobNormalGenerator(
            FieldLifeTemplate lifeTemplate,
            MobTemplate mobTemplate,
            DateTime now
        ) : base(lifeTemplate, mobTemplate)
        {
            NextRegenCheck = now;
        }

        public override bool Check(DateTime now, IField field)
        {
            if (now < NextRegenCheck) return false;

            var bounds = new Rect2D(
                new Point2D(
                    LifeTemplate.Position.X,
                    LifeTemplate.Position.Y
                ),
                new Size2D(200, 200)
            );

            NextRegenCheck = NextRegenCheck.AddSeconds(7);

            return !field
                    .GetObjects<IFieldObjMob>()
                    .Any(m => bounds.Contains(m.Position));
        }
    }
}
