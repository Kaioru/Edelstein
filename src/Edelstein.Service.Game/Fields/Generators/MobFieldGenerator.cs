using System;
using System.Threading.Tasks;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Service.Game.Fields.Objects.Mobs;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class MobFieldGenerator : IFieldGenerator
    {
        private readonly FieldLifeTemplate _lifeTemplate;
        private readonly MobTemplate _mobTemplate;

        public TimeSpan RegenInterval => TimeSpan.FromSeconds(_lifeTemplate.MobTime);
        public DateTime RegenAfter { get; private set; }
        public IFieldGeneratedObj Generated { get; private set; }

        public MobFieldGenerator(FieldLifeTemplate lifeTemplate, MobTemplate mobTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _mobTemplate = mobTemplate;
        }

        public async Task Reset()
        {
            var interval = TimeSpan.Zero;

            if (RegenInterval.Seconds > 0)
            {
                var random = new Random();
                var buffer = 7 * RegenInterval.Seconds / 10;

                interval = TimeSpan.FromSeconds(
                    13 * RegenInterval.Seconds / 10 + random.Next(buffer)
                );
            }

            Generated = null;
            RegenAfter = DateTime.Now.Add(interval);
        }

        public async Task Generate(IField field)
        {
            var mob = new FieldMob(_mobTemplate, _lifeTemplate.Left)
            {
                Generator = this,
                Position = _lifeTemplate.Position,
                Foothold = (short) _lifeTemplate.FH,
                HomeFoothold = (short) _lifeTemplate.FH
            };

            Generated = mob;
            await field.Enter(mob);
        }
    }
}