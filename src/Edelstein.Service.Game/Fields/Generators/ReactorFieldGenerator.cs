using System;
using System.Threading.Tasks;
using Edelstein.Provider.Templates.Field.Reactor;
using Edelstein.Service.Game.Fields.Objects.Reactors;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class ReactorFieldGenerator : AbstractFieldGenerator
    {
        private readonly FieldReactorTemplate _fieldReactorTemplate;
        private readonly ReactorTemplate _reactorTemplate;

        public override TimeSpan RegenInterval => TimeSpan.FromSeconds(_fieldReactorTemplate.ReactorTime);

        public ReactorFieldGenerator(FieldReactorTemplate fieldReactorTemplate, ReactorTemplate reactorTemplate)
        {
            _fieldReactorTemplate = fieldReactorTemplate;
            _reactorTemplate = reactorTemplate;
        }

        public override async Task Generate(IField field)
        {
            var reactor = new FieldReactor(_reactorTemplate, _fieldReactorTemplate.F)
            {
                Generator = this,
                Position = _fieldReactorTemplate.Position
            };

            Generated = reactor;
            await field.Enter(reactor);
        }
    }
}