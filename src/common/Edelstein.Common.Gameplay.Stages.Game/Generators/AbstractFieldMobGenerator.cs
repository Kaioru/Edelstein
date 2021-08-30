using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Common.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public abstract class AbstractFieldMobGenerator : Protocol.Gameplay.Stages.Game.Generators.AbstractFieldMobGenerator
    {
        protected FieldLifeTemplate LifeTemplate { get; }
        protected MobTemplate MobTemplate { get; }

        public AbstractFieldMobGenerator(FieldLifeTemplate lifeTemplate, MobTemplate mobTemplate)
        {
            LifeTemplate = lifeTemplate;
            MobTemplate = mobTemplate;
        }

        public abstract bool Check(DateTime now, IField field);

        public Task Generate(IField field)
            => field.Enter(new FieldObjMob(MobTemplate, LifeTemplate.Left)
            {
                Position = LifeTemplate.Position,
                Foothold = field.GetFoothold(LifeTemplate.FH)
            });
    }
}