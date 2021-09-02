using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using DotNet.Globbing;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Common.Gameplay.Templating;
using MoreLinq;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Util
{
    public record TemplateCommandIndex(
        int ID,
        string SearchString,
        string DisplayString
    );

    public class TemplateCommandArgs : CommandArgs
    {
        [ArgPosition(0), ArgRequired]
        [ArgDescription("The search text")]
        public string Search { get; set; }
    }

    public abstract class AbstractTemplateCommand<TTemplate> : AbstractTemplateCommand<TTemplate, TemplateCommandArgs> where TTemplate : class, ITemplate
    {
        protected AbstractTemplateCommand(ITemplateRepository<TTemplate> templates) : base(templates)
        {
        }
    }

    public abstract class AbstractTemplateCommand<TTemplate, TArgs> : AbstractCommand<TArgs>
        where TTemplate : class, ITemplate
        where TArgs : TemplateCommandArgs
    {
        private readonly ITemplateRepository<TTemplate> _templates;
        private readonly GlobOptions _options = new() { Evaluation = { CaseInsensitive = true } };

        protected AbstractTemplateCommand(
            ITemplateRepository<TTemplate> templates
        )
        {
            _templates = templates;
        }

        protected abstract Task<IEnumerable<TemplateCommandIndex>> Indices();
        protected abstract Task Execute(IFieldObjUser user, TTemplate template, TArgs args);

        public override async Task Execute(IFieldObjUser user, TArgs args)
        {
            var stopwatch = new Stopwatch();

            await user.Message($"Searching for '{args.Search}', this might take awhile..");
            stopwatch.Start();

            var glob = Glob.Parse(args.Search, _options);
            var data = await Indices();

            var results = data
                .Where(d => glob.IsMatch(d.SearchString))
                .DistinctBy(d => d.ID)
                .ToList();
            var elapsed = stopwatch.Elapsed;

            if (results.Any())
            {
                var templateID = await user.Prompt(target =>
                {
                    var maxPerPage = 6;
                    var minPage = 1;
                    var maxPage = (int)Math.Ceiling((double)results.Count / maxPerPage);
                    var currentPage = 1;

                    while (true)
                    {
                        var items = results
                            .Skip(maxPerPage * (currentPage - 1))
                            .Take(maxPerPage)
                            .ToList();
                        var menu = items.ToDictionary(
                            r => r.ID,
                            r => $"{r.DisplayString} ({r.ID})"
                        );

                        if (currentPage < maxPage) menu.Add(-10, "#rNext page#k");
                        if (currentPage > minPage) menu.Add(-20, "#rPrevious page#k");

                        var selection = target.AskMenu($"Found {results.Count} results for '{args.Search}' in {elapsed.TotalSeconds:F} seconds (page {currentPage} of {maxPage})", menu);

                        if (selection == -10) { currentPage++; continue; }
                        if (selection == -20) { currentPage--; continue; }

                        return selection;
                    }
                });
                var template = await _templates.Retrieve(templateID);

                if (template == null)
                {
                    await user.Message($"The template for {templateID} does not exist");
                    return;
                }

                await Execute(user, template, args);
                return;
            }

            await user.Message($"No search results found for '{args.Search}'");
        }
    }
}
