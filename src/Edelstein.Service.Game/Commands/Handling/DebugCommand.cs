using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class DebugCommand : AbstractCommand<object>
    {
        public override string Name => "Debug";
        public override string Description => "Debugs various information";

        public DebugCommand(Parser parser) : base(parser)
        {
            Commands.Add(new DebugFieldCommand(parser));
        }

        protected override Task Execute(FieldUser sender, object option)
        {
            return Process(sender, new Queue<string>(new[] {"--help"}));
        }
    }

    public class DebugFieldCommand : AbstractCommand<object>
    {
        public override string Name => "Field";
        public override string Description => "Debugs various field information";

        public DebugFieldCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var field = sender.Field;
            var stringTemplate = sender.Service.TemplateManager.Get<FieldStringTemplate>(field.ID);

            await sender.Message($"{stringTemplate.StreetName}: {stringTemplate.MapName} ({field.ID})");
            await sender.Message($"Object count: {field.GetObjects().Count()} (Controlled: {field.GetControlledObjects(sender).Count()} / {field.GetObjects<IFieldControlledObj>().Count()})");
        }
    }
}