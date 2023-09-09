using System.Diagnostics;
using DotNet.Globbing;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Templates;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands;

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
    protected AbstractTemplateCommand(ITemplateManager<TTemplate> templates) : base(templates)
    {
    }
}

public abstract class AbstractTemplateCommand<TTemplate, TArgs> : AbstractCommand<TArgs>
    where TTemplate : class, ITemplate
    where TArgs : TemplateCommandArgs
{
    private readonly ITemplateManager<TTemplate> _templates;
    private readonly GlobOptions _options = new() { Evaluation = { CaseInsensitive = true } };

    protected AbstractTemplateCommand(
        ITemplateManager<TTemplate> templates
    )
    {
        _templates = templates;
    }

    protected abstract Task<IEnumerable<TemplateCommandIndex>> Indices();
    protected abstract Task Execute(IFieldUser user, TTemplate template, TArgs args);

    protected override async Task Execute(IFieldUser user, TArgs args)
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
            }, -1);
            if (templateID == -1) return;
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
