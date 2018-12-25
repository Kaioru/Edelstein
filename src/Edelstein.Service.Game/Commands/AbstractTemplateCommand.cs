using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using DotNet.Globbing;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands
{
    public abstract class AbstractTemplateCommand<TTemplate, TString, TOption> : AbstractGameCommand<TOption>
        where TTemplate : ITemplate
        where TString : IStringTemplate
        where TOption : TemplateCommandOption
    {
        private readonly GlobOptions _globOptions = new GlobOptions {Evaluation = {CaseInsensitive = true}};

        public AbstractTemplateCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task ExecuteAfter(FieldUser user, TOption option)
        {
            var templateManager = user.Socket.WvsGame.TemplateManager;
            var templateStrings = templateManager.GetAll<TString>().ToList();
            var templateID = option.TemplateID;

            if (!string.IsNullOrEmpty(option.Search))
            {
                var glob = Glob.Parse(option.Search, _globOptions);
                var results = templateStrings
                    .Where(p => glob.IsMatch(p.Name))
                    .ToList();

                if (!results.Any())
                    results = templateStrings
                        .Where(p => p.Name.ToLower().Contains(option.Search.ToLower()))
                        .ToList();

                if (results.Any())
                {
                    templateID = await user.Prompt<int>((self, target) => self.AskMenu(
                        $"Here are the results for '{option.Search}'",
                        results.ToDictionary(
                            r => r.ID,
                            r => r.Name
                        )
                    ), 0);
                }
            }

            if (templateID == null) return;
            await ExecuteAfter(user, templateManager.Get<TTemplate>(templateID.Value), option);
        }

        protected abstract Task ExecuteAfter(FieldUser user, TTemplate template, TOption option);
    }

    public class TemplateCommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template with string.")]
        public string Search { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The template ID.")]
        public int? TemplateID { get; set; }
    }
}