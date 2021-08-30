using System;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob;
using System.Threading.Tasks;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public class FieldMobTimedGenerator : AbstractFieldMobGenerator
    {
        private DateTime NextRegen { get; set; }
        private IFieldObjMob Mob { get; set; }

        public FieldMobTimedGenerator(
            FieldLifeTemplate lifeTemplate,
            MobTemplate mobTemplate
        ) : base(lifeTemplate, mobTemplate)
        {
        }

        public override bool Check(DateTime now, IField field)
        {
            if (Mob != null)
            {
                if (Mob.Field == null)
                {
                    var random = new Random();
                    var buffer = 7 * LifeTemplate.MobTime / 10;
                    var interval = TimeSpan.FromSeconds(
                        13 * LifeTemplate.MobTime / 10 + random.Next(buffer)
                    );

                    NextRegen = NextRegen.Add(interval);
                    Mob = null;
                }

                return false;
            }

            return NextRegen < now;
        }

        public override async Task Generate(IField field)
        {
            var mob = new FieldObjMob(MobTemplate, LifeTemplate.Left)
            {
                Position = LifeTemplate.Position,
                Foothold = field.GetFoothold(LifeTemplate.FH)
            };

            await field.Enter(mob, () => mob.GetEnterFieldPacket(FieldObjMobAppearType.Regen));

            Mob = mob;
        }
    }
}
