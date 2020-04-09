using System;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Templates.Reactor;
using Edelstein.Service.Game.Fields.Objects.Reactor;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class FieldGeneratorReactor : AbstractFieldGenerator
    {
        private readonly FieldReactorTemplate _lifeTemplate;
        private readonly ReactorTemplate _reactorTemplate;
        private DateTime RegenAfter { get; set; }

        public FieldGeneratorReactor(FieldReactorTemplate lifeTemplate, ReactorTemplate reactorTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _reactorTemplate = reactorTemplate;
        }

        public override bool Available(IField field, bool reset = false)
            => Generated.Count == 0 &&
               DateTime.UtcNow >= RegenAfter;

        public override async Task Generate(IField field)
        {
            var reactor = new FieldReactor(_reactorTemplate, _lifeTemplate.F)
            {
                Generator = this,
                Position = _lifeTemplate.Position
            };

            Generated.Add(reactor);
            await field.Enter(reactor);
        }

        public override async Task Reset(IFieldGeneratorObj obj)
        {
            obj.Generator = null;
            Generated.Remove(obj);

            var random = new Random();
            var buffer = 7 * _lifeTemplate.ReactorTime / 10;
            var interval = TimeSpan.FromSeconds(
                13 * _lifeTemplate.ReactorTime / 10 + random.Next(buffer)
            );

            RegenAfter = DateTime.UtcNow.Add(interval);
        }
    }
}