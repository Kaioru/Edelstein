using System;
using System.Threading.Tasks;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Service.Game.Fields.Objects.Mobs;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class MobFieldGenerator : AbstractFieldGenerator
    {
        private readonly FieldLifeTemplate _lifeTemplate;
        private readonly MobTemplate _mobTemplate;

        public override TimeSpan RegenInterval => TimeSpan.FromSeconds(_lifeTemplate.MobTime);

        public MobFieldGenerator(FieldLifeTemplate lifeTemplate, MobTemplate mobTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _mobTemplate = mobTemplate;
        }

        public override async Task Generate(IField field)
        {
            var mob = new FieldMob(_mobTemplate, _lifeTemplate.Left)
            {
                Position = _lifeTemplate.Position,
                Foothold = (short) _lifeTemplate.FH,
                HomeFoothold = (short) _lifeTemplate.FH
            };

            if (RegenInterval.TotalSeconds > 0)
            {
                mob.Generator = this;
                Generated = mob;
            }
            await field.Enter(mob, () => mob.GetEnterFieldPacket(MobAppearType.Regen));
        }
    }
}