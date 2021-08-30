using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Generators;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public abstract class AbstractFieldMobGenerator : IFieldGenerator
    {
        protected FieldLifeTemplate LifeTemplate { get; }
        protected MobTemplate MobTemplate { get; }

        public AbstractFieldMobGenerator(FieldLifeTemplate lifeTemplate, MobTemplate mobTemplate)
        {
            LifeTemplate = lifeTemplate;
            MobTemplate = mobTemplate;
        }

        public abstract bool Check(DateTime now, IField field);

        public virtual Task Generate(IField field)
        {
            var mob = new FieldObjMob(MobTemplate, LifeTemplate.Left)
            {
                Position = LifeTemplate.Position,
                Foothold = field.GetFoothold(LifeTemplate.FH)
            };

            return field.Enter(mob, () => mob.GetEnterFieldPacket(FieldObjMobAppearType.Regen));
        }
    }
}