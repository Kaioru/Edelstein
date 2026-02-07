using System.Buffers;
using System.Collections.Frozen;
using System.Runtime.CompilerServices;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Templates;
using Gma.DataStructures.StringSearch;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands;

public enum TemplateCommandIndexKind : byte
{
    Default = 0,
    Description = 1
}

/// <summary>
/// Index entry for template command search.
/// SearchString is stored as-is; normalization is applied at query/build time.
/// </summary>
public readonly record struct TemplateCommandIndex(int ID, string SearchString, string DisplayString, TemplateCommandIndexKind Kind)
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TemplateCommandIndex Create(int id, string? searchString, string displayString)
        => new(id, searchString ?? string.Empty, displayString, TemplateCommandIndexKind.Default);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TemplateCommandIndex CreateDescription(int id, string? searchString, string displayString)
        => new(id, searchString ?? string.Empty, displayString, TemplateCommandIndexKind.Description);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static TemplateCommandIndex CreateFromId(int id, string displayString)
        => new(id, id.ToString(), displayString, TemplateCommandIndexKind.Default);
}

public class TemplateCommandArgs : CommandArgs
{
    [ArgPosition(0), ArgRequired]
    [ArgDescription("The search text")]
    public string Search { get; set; } = string.Empty;
}

/// <summary>
/// Base class for template search commands with default argument handling.
/// </summary>
public abstract class AbstractTemplateCommand<TTemplate> : AbstractTemplateCommand<TTemplate, TemplateCommandArgs>
    where TTemplate : class, ITemplate
{
    protected AbstractTemplateCommand(ITemplateManager<TTemplate> templates) : base(templates) { }
}

/// <summary>
/// Base class for searchable template commands with trie-based indexing and pagination.
/// Supports both numeric ID lookup and text search with lazy trie construction.
/// </summary>
public abstract class AbstractTemplateCommand<TTemplate, TArgs> : AbstractCommand<TArgs>, IIndexedCommand
    where TTemplate : class, ITemplate
    where TArgs : TemplateCommandArgs
{
    private const int MenuNextPage = -10;
    private const int MenuPrevPage = -20;
    private const int MenuCancelled = -1;
    private const int MaxResultsPerPage = 6;
    private const int TrieMinQueryLength = 3;
    private const int MaxConcurrentTrieBuilds = 3;
    private const int InitialResultCapacity = 64;
    private const int MaxStackAllocLength = 256;

    [ThreadStatic]
    private static HashSet<int>? t_seenIdsPool;

    private readonly ITemplateManager<TTemplate> _templates;
    private readonly UkkonenTrie<int> _trie;
    private readonly SemaphoreSlim _indexLock = new(1, 1);
    private static readonly int TrieBuildConcurrency = Math.Min(MaxConcurrentTrieBuilds, Environment.ProcessorCount);
    private static readonly SemaphoreSlim TrieBuildLock = new(TrieBuildConcurrency, TrieBuildConcurrency);

    private readonly record struct IndexEntry(int ID, string SearchString, TemplateCommandIndexKind Kind);
    private readonly record struct MatchResult(int ID, string? DisplayOverride);

    /// <summary>
    /// Manages a pooled array for collecting match results without per-search allocations.
    /// </summary>
    private sealed class PooledResultList : IDisposable
    {
        private MatchResult[] _array;
        private int _count;

        public PooledResultList(int initialCapacity)
        {
            _array = ArrayPool<MatchResult>.Shared.Rent(initialCapacity);
            _count = 0;
        }

        public int Count => _count;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(MatchResult result)
        {
            if (_count >= _array.Length)
                Grow();
            _array[_count++] = result;
        }

        public MatchResult[] ToArray()
        {
            var result = new MatchResult[_count];
            _array.AsSpan(0, _count).CopyTo(result);
            return result;
        }

        private void Grow()
        {
            var newArray = ArrayPool<MatchResult>.Shared.Rent(_array.Length * 2);
            _array.AsSpan(0, _count).CopyTo(newArray);
            ArrayPool<MatchResult>.Shared.Return(_array);
            _array = newArray;
        }

        public void Dispose() => ArrayPool<MatchResult>.Shared.Return(_array);
    }

    private IndexEntry[] _linearEntries = [];
    private FrozenDictionary<int, string> _displayById = FrozenDictionary<int, string>.Empty;
    private volatile bool _trieEnabled;
    private volatile bool _trieDisabled; // Set to true if trie failed and should not be used

    private IndexingStatus? _indexingStatus;
    private int _isIndexed; // 0 = not indexed, 1 = indexed (using int for Interlocked)
    private int _isTrieBuilt; // 0 = not built, 1 = built (using int for Interlocked)

    protected AbstractTemplateCommand(ITemplateManager<TTemplate> templates)
    {
        _templates = templates;
        _trie = new UkkonenTrie<int>(TrieMinQueryLength);
    }

    protected abstract Task<IReadOnlyList<TemplateCommandIndex>> Indices();
    protected abstract Task Execute(IFieldUser user, TTemplate template, TArgs args);

    public async Task Index(IndexingStatus status)
    {
        if (Volatile.Read(ref _isIndexed) == 1) return;

        await _indexLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (Volatile.Read(ref _isIndexed) == 1) return;

            _indexingStatus = status;
            status.SetLoading(Name);

            var indices = await Indices().ConfigureAwait(false);

            var count = indices.Count;
            var linear = new IndexEntry[count];
            var displayBuilder = new Dictionary<int, string>(capacity: Math.Min(count, 16_384));

            for (var i = 0; i < count; i++)
            {
                var index = indices[i];
                linear[i] = new IndexEntry(index.ID, index.SearchString, index.Kind);

                if (!string.IsNullOrWhiteSpace(index.DisplayString))
                    displayBuilder.TryAdd(index.ID, index.DisplayString);
            }

            _linearEntries = linear;
            _displayById = displayBuilder.Count == 0
                ? FrozenDictionary<int, string>.Empty
                : displayBuilder.ToFrozenDictionary();

            Volatile.Write(ref _isIndexed, 1);
            status.SetCompleted(Name, count);
        }
        finally
        {
            _indexLock.Release();
        }
    }

    /// <summary>Lazily builds the trie on first search.</summary>
    private async Task EnsureTrieBuilt()
    {
        if (Volatile.Read(ref _isTrieBuilt) == 1 || _trieDisabled) return;

        await TrieBuildLock.WaitAsync().ConfigureAwait(false);
        try
        {
            if (Volatile.Read(ref _isTrieBuilt) == 1 || _trieDisabled) return;

            _indexingStatus?.SetBuilding(Name, _linearEntries.Length);

            var descriptionIndices = 0;
            var normalizedNull = 0;
            var normalizedTooShort = 0;
            var duplicateKeys = 0;
            var addedKeys = 0;
            var addArgumentExceptions = 0;
            var addOutOfRangeExceptions = 0;
            var addNullReferenceExceptions = 0;

            var buildStartTick = Environment.TickCount64;
            var count = _linearEntries.Length;
            var uniqueKeySet = new HashSet<string>(Math.Min(count, 32_768), StringComparer.Ordinal);

            for (var i = 0; i < count; i++)
            {
                var entry = _linearEntries[i];
                if (entry.Kind == TemplateCommandIndexKind.Description)
                {
                    descriptionIndices++;
                    continue;
                }

                var keyLower = StringNormalizer.NormalizeKey(entry.SearchString, lowercase: true);
                if (keyLower is null)
                {
                    normalizedNull++;
                    continue;
                }

                if (keyLower.Length < TrieMinQueryLength)
                {
                    normalizedTooShort++;
                    continue;
                }

                if (!uniqueKeySet.Add(keyLower))
                {
                    duplicateKeys++;
                    continue;
                }

                try
                {
                    _trie.Add(keyLower, entry.ID);
                    addedKeys++;
                }
                catch (ArgumentOutOfRangeException)
                {
                    addOutOfRangeExceptions++;
                }
                catch (ArgumentException)
                {
                    addArgumentExceptions++;
                }
                catch (NullReferenceException)
                {
                    addNullReferenceExceptions++;
                }
            }

            var buildElapsedMs = Environment.TickCount64 - buildStartTick;

            _trieEnabled = addedKeys > 0;

            _indexingStatus?.SetTrieTelemetry(new TrieIndexTelemetry(
                CommandName: Name,
                TotalIndices: count,
                DescriptionIndices: descriptionIndices,
                NormalizedNull: normalizedNull,
                NormalizedTooShort: normalizedTooShort,
                NormalizedTooLong: 0,
                DuplicateKeys: duplicateKeys,
                AddedKeys: addedKeys,
                AddArgumentExceptions: addArgumentExceptions,
                AddOutOfRangeExceptions: addOutOfRangeExceptions,
                AddNullReferenceExceptions: addNullReferenceExceptions,
                RetrieveExceptions: 0,
                BuildElapsedMs: buildElapsedMs,
                TrieEnabled: _trieEnabled));

            Volatile.Write(ref _isTrieBuilt, 1);
        }
        finally
        {
            TrieBuildLock.Release();
        }
    }

    protected override async Task Execute(IFieldUser user, TArgs args)
    {
        if (Volatile.Read(ref _isIndexed) != 1)
        {
            var message = _indexingStatus?.GetProgressMessage()
                ?? "Templates have not finished indexing yet, please try again later..";
            await user.Message(message);
            return;
        }

        var startTick = Environment.TickCount64;
        await user.Message($"Searching for '{args.Search}' ..");

        var normalizedSearch = StringNormalizer.NormalizeKey(args.Search);
        if (normalizedSearch is null)
        {
            await user.Message($"No search results found for '{args.Search}'");
            return;
        }

        var isNumericSearch = IsAllDigits(normalizedSearch.AsSpan());

        using var results = new PooledResultList(InitialResultCapacity);
        var seenIds = t_seenIdsPool ??= new HashSet<int>(InitialResultCapacity);
        seenIds.Clear();

        // Exact ID match first - ensures "100" shows item ID 100 before "Level 100 Sword"
        if (isNumericSearch && int.TryParse(normalizedSearch, out var exactId))
        {
            var exactMatch = await _templates.Retrieve(exactId);
            if (exactMatch != null)
            {
                var displayName = _displayById.TryGetValue(exactId, out var knownDisplay) && !string.IsNullOrWhiteSpace(knownDisplay)
                    ? knownDisplay
                    : "EXACT-ID-MATCH";

                var displayString = string.Create(displayName.Length + 4, displayName, static (span, name) =>
                {
                    "#e".AsSpan().CopyTo(span);
                    name.AsSpan().CopyTo(span[2..]);
                    "#n".AsSpan().CopyTo(span[(2 + name.Length)..]);
                });

                results.Add(new MatchResult(exactId, displayString));
                seenIds.Add(exactId);
            }
        }

        var canUseTrie = normalizedSearch.Length >= TrieMinQueryLength && !_trieDisabled;

        if (canUseTrie)
        {
            await EnsureTrieBuilt();
            var trieSearchSucceeded = true;

            if (_trieEnabled)
            {
                try
                {
                    var searchLower = ToLowerStackAlloc(normalizedSearch);

                    foreach (var id in _trie.Retrieve(searchLower))
                    {
                        if (seenIds.Add(id))
                            results.Add(new MatchResult(id, null));
                    }
                }
                catch
                {
                    trieSearchSucceeded = false;
                    _trieDisabled = true;
                    _indexingStatus?.IncrementTrieRetrieveException(Name);
                }
            }
            else
            {
                trieSearchSucceeded = false;
            }

            // Trie success: only search Description entries; Default entries already in trie
            var kindFilter = trieSearchSucceeded ? TemplateCommandIndexKind.Description : (TemplateCommandIndexKind?)null;
            AddLinearMatches(results, seenIds, normalizedSearch, kindFilter);
        }
        else
        {
            AddLinearMatches(results, seenIds, normalizedSearch, kindFilter: null);
        }

        var elapsedMs = Environment.TickCount64 - startTick;

        if (results.Count > 0)
        {
            var resultsCopy = results.ToArray();

            var templateID = await user.Prompt(target =>
            {
                var maxPage = (resultsCopy.Length + MaxResultsPerPage - 1) / MaxResultsPerPage;
                var currentPage = 1;
                var menu = new Dictionary<int, string>(MaxResultsPerPage + 2);

                while (true)
                {
                    menu.Clear();
                    var skipCount = MaxResultsPerPage * (currentPage - 1);

                    for (var i = skipCount; i < resultsCopy.Length && i < skipCount + MaxResultsPerPage; i++)
                    {
                        var r = resultsCopy[i];
                        var display = r.DisplayOverride;
                        if (string.IsNullOrWhiteSpace(display))
                            display = _displayById.TryGetValue(r.ID, out var d) && !string.IsNullOrWhiteSpace(d)
                                ? d
                                : r.ID.ToString();

                        menu[r.ID] = $"{display} ({r.ID})";
                    }

                    if (currentPage < maxPage) menu[MenuNextPage] = "#rNext page#k";
                    if (currentPage > 1) menu[MenuPrevPage] = "#rPrevious page#k";

                    var selection = target.AskMenu(
                        $"Found {resultsCopy.Length} results for '{args.Search}' in {elapsedMs}ms (page {currentPage} of {maxPage})",
                        menu);

                    switch (selection)
                    {
                        case MenuNextPage: currentPage++; continue;
                        case MenuPrevPage: currentPage--; continue;
                        default: return selection;
                    }
                }
            }, MenuCancelled);

            if (templateID == MenuCancelled) return;

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

    private static string ToLowerStackAlloc(string input)
    {
        var length = input.Length;
        Span<char> buffer = length <= MaxStackAllocLength
            ? stackalloc char[length]
            : new char[length];

        for (var i = 0; i < length; i++)
            buffer[i] = char.ToLowerInvariant(input[i]);

        return new string(buffer);
    }

    private void AddLinearMatches(
        PooledResultList results,
        HashSet<int> seenIds,
        ReadOnlySpan<char> query,
        TemplateCommandIndexKind? kindFilter)
    {
        for (var i = 0; i < _linearEntries.Length; i++)
        {
            var entry = _linearEntries[i];

            if (kindFilter.HasValue && entry.Kind != kindFilter.Value)
                continue;

            if (!entry.SearchString.AsSpan().Contains(query, StringComparison.OrdinalIgnoreCase))
                continue;

            if (seenIds.Add(entry.ID))
                results.Add(new MatchResult(entry.ID, null));
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static bool IsAllDigits(ReadOnlySpan<char> span)
    {
        if (span.IsEmpty) return false;

        foreach (var c in span)
            if (!char.IsAsciiDigit(c))
                return false;

        return true;
    }
}
