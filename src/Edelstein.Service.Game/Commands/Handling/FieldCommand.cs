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

        protected override Task ExecuteAfter(FieldUser user, FieldTemplate template, TemplateCommandOption option)
        {
            var fieldManager = user.Socket.WvsGame.FieldManager;
            var field = fieldManager.Get(template.ID);

            return field.Enter(user);
        }
    }
}