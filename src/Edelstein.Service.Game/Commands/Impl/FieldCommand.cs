using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Templates.Fields;
using Edelstein.Core.Templates.Strings;
using Edelstein.Service.Game.Commands.Util;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Impl
{
    public class FieldCommand : AbstractTemplateCommand<
        FieldTemplate,
        FieldStringTemplate,
        DefaultTemplateCommandContext
    >
    {
        public override string Name => "Field";
        public override string Description => "Transfers to a field from the specified options";


        public FieldCommand(Parser parser) : base(parser)
        {
            Aliases.Add("Map");
            Aliases.Add("Warp");
        }

        protected override async Task Run(
            FieldUser sender,
            FieldTemplate template,
            DefaultTemplateCommandContext ctx
        )
        {
            var fieldManager = sender.Service.FieldManager;
            var field = fieldManager.Get(template.ID);

            await field.Enter(sender);
        }
    }
}