using System.Linq;
using System.Threading.Tasks;
using CommandLine;
using DotNet.Globbing;
using Edelstein.Core.Templates;
using Edelstein.Provider;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Impl.Util
{
    public abstract class AbstractTemplateCommand<TTemplate, TString, TContext> : AbstractCommand<TContext>
        where TTemplate : IDataTemplate
        where TString : IDataStringTemplate
        where TContext : DefaultTemplateCommandContext
    {
        private readonly GlobOptions _globOptions = new GlobOptions
        {
            Evaluation = {CaseInsensitive = true}
        };

        protected AbstractTemplateCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Run(FieldUser sender, TContext ctx)
        {
            var templateManager = sender.Service.TemplateManager;
            var templateStrings = templateManager.GetAll<TString>().ToList();
            var templateID = ctx.TemplateID;

            if (!string.IsNullOrEmpty(ctx.Search))
            {
                var glob = Glob.Parse(ctx.Search, _globOptions);
                var results = templateStrings
                    .Where(p => glob.IsMatch(p.Name))
                    .ToList();

                if (!results.Any())
                    results = templateStrings
                        .Where(p => p.Name.ToLower().Contains(ctx.Search.ToLower()))
                        .ToList();

                if (results.Any())
                {
                    templateID = await sender.Prompt<int>(target => target.AskMenu(
                        $"Here are the results for '{ctx.Search}'",
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

            await Run(sender, template, ctx);
        }

        protected abstract Task Run(FieldUser sender, TTemplate template, TContext ctx);
    }

    public class DefaultTemplateCommandContext : DefaultCommandContext
    {
        [Option('s', "search", HelpText = "Searches for the template with string.")]
        public string Search { get; set; }

        [Value(0, MetaName = "templateID", HelpText = "The template ID.")]
        public int? TemplateID { get; set; }
    }
}