using System;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields.Life;
using Edelstein.Core.Templates.Mob;
using Edelstein.Service.Game.Fields.Objects.Mob;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class FieldGeneratorMob : AbstractFieldGenerator
    {
        private readonly FieldLifeTemplate _lifeTemplate;
        private readonly MobTemplate _mobTemplate;
        private DateTime RegenAfter { get; set; }

        public FieldGeneratorMob(FieldLifeTemplate lifeTemplate, MobTemplate mobTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _mobTemplate = mobTemplate;
        }

        public override bool Available(IField field, bool reset = false)
        {
            if (_lifeTemplate.MobTime == 0)
            {
                var bounds = new Rectangle(
                    new Point(
                        _lifeTemplate.Position.X - 100,
                        _lifeTemplate.Position.Y - 100
                    ),
                    new Size(200, 200)
                );

                return !field
                    .GetObjects<FieldMob>()
                    .Any(m => bounds.Contains(m.Position));
            }

            if (_lifeTemplate.MobTime > 0)
                return Generated.Count == 0 &&
                       RegenAfter <= DateTime.UtcNow;

            return reset;
        }

        public override async Task Generate(IField field)
        {
            var mob = new FieldMob(_mobTemplate, _lifeTemplate.Left)
            {
                Generator = this,
                Position = _lifeTemplate.Position,
                Foothold = (short) _lifeTemplate.FH,
                HomeFoothold = (short) _lifeTemplate.FH
            };

            Generated.Add(mob);
            await field.Enter(mob, () => mob.GetEnterFieldPacket(MobAppearType.Regen));
        }

        public override async Task Reset(IFieldGeneratorObj obj)
        {
            obj.Generator = null;
            Generated.Remove(obj);

            if (_lifeTemplate.MobTime > 0)
            {
                var random = new Random();
                var buffer = 7 * _lifeTemplate.MobTime / 10;
                var interval = TimeSpan.FromSeconds(
                    13 * _lifeTemplate.MobTime / 10 + random.Next(buffer)
                );

                RegenAfter = DateTime.UtcNow.Add(interval);
            }
        }
    }
}