using System.Collections.Immutable;
using System.Diagnostics;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Templates;
using Gma.DataStructures.StringSearch;
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

public abstract class AbstractTemplateCommand<TTemplate, TArgs> : AbstractCommand<TArgs>, IIndexedCommand
    where TTemplate : class, ITemplate
    where TArgs : TemplateCommandArgs
{
    private readonly ITemplateManager<TTemplate> _templates;
    private readonly ITrie<TemplateCommandIndex> _trie;
    private bool isIndexing;
    private bool isIndexed;

    protected AbstractTemplateCommand(
        ITemplateManager<TTemplate> templates
    )
    {
        _templates = templates;
        _trie = new UkkonenTrie<TemplateCommandIndex>();
    }

    protected abstract Task<IEnumerable<TemplateCommandIndex>> Indices();
    protected abstract Task Execute(IFieldUser user, TTemplate template, TArgs args);

    public async Task Index()
    {
        if (isIndexed || isIndexing) return;
        isIndexing = true;
        foreach (var index in await Indices())
            _trie.Add(index.SearchString.ToLower(), index);
        isIndexing = false;
        isIndexed = true;
    }
    
    protected override async Task Execute(IFieldUser user, TArgs args)
    {
        if (isIndexing || !isIndexed)
        {
            await user.Message("Templates have not finished indexing yet, please try again later..");
            return;
        }
        
        var stopwatch = new Stopwatch();

        await user.Message($"Searching for '{args.Search}', this might take awhile..");
        stopwatch.Start();

        var results = _trie
            .Retrieve(args.Search.ToLower())
            .DistinctBy(d => d.ID)
            .ToImmutableArray();
        var elapsed = stopwatch.Elapsed;

        if (args.Search.All(char.IsDigit) && !results.Any())
        {
            var searchID = Convert.ToInt32(args.Search);
            var search = await _templates.Retrieve(searchID);
            
            if (search != null)
                results.Add(new TemplateCommandIndex(searchID, searchID.ToString(), "NO-NAME"));
        }

        if (results.Any())
        {
            var templateID = await user.Prompt(target =>
            {
                var maxPerPage = 6;
                var minPage = 1;
                var maxPage = (int)Math.Ceiling((double)results.Length / maxPerPage);
                var currentPage = 1;

                while (true)
                {
                    var items = results
                        .Skip(maxPerPage * (currentPage - 1))
                        .Take(maxPerPage)
                        .ToImmutableArray();
                    var menu = items.ToDictionary(
                        r => r.ID,
                        r => $"{r.DisplayString} ({r.ID})"
                    );

                    if (currentPage < maxPage) menu.Add(-10, "#rNext page#k");
                    if (currentPage > minPage) menu.Add(-20, "#rPrevious page#k");

                    var selection = target.AskMenu($"Found {results.Length} results for '{args.Search}' in {elapsed.TotalMilliseconds:F2}ms (page {currentPage} of {maxPage})", menu);

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
