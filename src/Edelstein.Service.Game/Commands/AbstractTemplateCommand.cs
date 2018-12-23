using System.Threading.Tasks;
using CommandLine;
using DotNet.Globbing;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Commands
{
    public abstract class AbstractTemplateCommand<T> : AbstractGameCommand<TemplateCommandOption>
        where T : ITemplate
    {
        private readonly GlobOptions _globOptions = new GlobOptions {Evaluation = {CaseInsensitive = true}};

        public AbstractTemplateCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(FieldUser user, TemplateCommandOption option)
        {
            var templateManager = user.Socket.WvsGame.TemplateManager;
            var templateID = option.TemplateID;

            if (templateID == null) return;
            await ExecuteAfter(user, templateManager.Get<T>(templateID.Value), option);
        }

        protected abstract Task ExecuteAfter(FieldUser user, T template, TemplateCommandOption option);
    }

    public class TemplateCommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template with string.")]
        public string Search { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The template ID.")]
        public int? TemplateID { get; set; }
    }
}