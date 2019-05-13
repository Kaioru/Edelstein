using System.Threading.Tasks;
using CommandLine;
using Edelstein.Provider.Templates.Field;
using Edelstein.Provider.Templates.String;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class FieldCommand : AbstractTemplateCommand<FieldTemplate, FieldStringTemplate, TemplateCommandOption>
    {
        public override string Name => "Field";
        public override string Description => "Transfers to a field from the specified options";

        public FieldCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(
            FieldUser sender,
            FieldTemplate template,
            TemplateCommandOption option
        )
        {
            var fieldManager = sender.Service.FieldManager;
            var field = fieldManager.Get(template.ID);

            await field.Enter(sender);
        }
    }
}