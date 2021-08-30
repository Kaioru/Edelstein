using System;
using Edelstein.Protocol.Gameplay.Stages.Game;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public class FieldMobTimedGenerator : AbstractFieldMobGenerator
    {
        private DateTime NextRegenCheck { get; set; }
        private DateTime? NextRegen { get; set; }

        private IFieldObjMob Mob { get; }

        public FieldMobTimedGenerator(
            FieldLifeTemplate lifeTemplate,
            MobTemplate mobTemplate,
            DateTime now
        ) : base(lifeTemplate, mobTemplate)
        {
            NextRegenCheck = now;
            NextRegen = now;
        }
        public override bool Check(DateTime now, IField field)
        {
            if (now < NextRegenCheck) return false;

            NextRegenCheck = NextRegenCheck.AddSeconds(7);

            if (Mob == null || Mob.Field == null)
            {
                if (NextRegen == null)
                {
                    var random = new Random();
                    var buffer = 7 * LifeTemplate.MobTime / 10;
                    var interval = TimeSpan.FromSeconds(
                        13 * LifeTemplate.MobTime / 10 + random.Next(buffer)
                    );

                    NextRegen = DateTime.UtcNow.Add(interval);
                }

                return now >= NextRegen;
            }
            else NextRegen = null;

            return false;
        }
    }
}
