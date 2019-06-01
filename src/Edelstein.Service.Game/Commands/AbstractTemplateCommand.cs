using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using DotNet.Globbing;
using Edelstein.Provider.Templates;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands
{
    public abstract class AbstractTemplateCommand<TTemplate, TString, TOption> : AbstractCommand<TOption>
        where TTemplate : ITemplate
        where TString : IStringTemplate
        where TOption : TemplateCommandOption
    {
        private readonly GlobOptions _globOptions = new GlobOptions
        {
            Evaluation = {CaseInsensitive = true}
        };

        protected AbstractTemplateCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, TOption option)
        {
            var templateManager = sender.Service.TemplateManager;
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
                    templateID = await sender.Prompt<int>(target => target.AskMenu(
                        $"Here are the results for '{option.Search}'",
                        results.ToDictionary(
                            r => r.ID,
                            r => $"{r.Name} ({r.ID})"
                        )
                    ));
                }
            }

            if (templateID == null) return;

            var template = templateManager.Get<TTemplate>(templateID.Value);

            if (template == null)
            {
                await sender.Message($"Unable to find template of ID: {templateID}");
                return;
            }

            await ExecuteAfter(sender, template, option);
        }

        protected abstract Task ExecuteAfter(FieldUser sender, TTemplate template, TOption option);
    }

    public class TemplateCommandOption
    {
        [Option('s', "search", HelpText = "Searches for the template with string.")]
        public string Search { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The template ID.")]
        public int? TemplateID { get; set; }
    }
}