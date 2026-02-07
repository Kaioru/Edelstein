using System.Collections.Concurrent;
using System.Runtime.CompilerServices;
using System.Text;

namespace Edelstein.Plugin.Rue.Commands;

/// <summary>
/// Represents the indexing lifecycle state.
/// Note: Enum values are ordered by lifecycle, NOT display priority.
/// </summary>
public enum IndexState : byte
{
    Pending = 0,
    Loading = 1,
    Building = 2,
    Completed = 3
}

public readonly record struct IndexProgress(string CommandName, IndexState State, int ItemCount = 0);

public readonly record struct TrieIndexTelemetry(
    string CommandName,
    int TotalIndices,
    int DescriptionIndices,
    int NormalizedNull,
    int NormalizedTooShort,
    int NormalizedTooLong,
    int DuplicateKeys,
    int AddedKeys,
    int AddArgumentExceptions,
    int AddOutOfRangeExceptions,
    int AddNullReferenceExceptions,
    int RetrieveExceptions,
    long BuildElapsedMs,
    bool TrieEnabled);

public sealed class IndexingStatus
{
    private readonly ConcurrentDictionary<string, IndexProgress> _progress = new();
    private readonly ConcurrentDictionary<string, TrieIndexTelemetry> _trieTelemetry = new();

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void Register(string commandName)
        => _progress.TryAdd(commandName, new IndexProgress(commandName, IndexState.Pending));

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetLoading(string commandName)
        => _progress.AddOrUpdate(
            commandName,
            static (name, _) => new IndexProgress(name, IndexState.Loading),
            static (_, existing, _) => existing with { State = IndexState.Loading },
            (object?)null);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetBuilding(string commandName, int itemCount)
        => _progress.AddOrUpdate(
            commandName,
            static (name, count) => new IndexProgress(name, IndexState.Building, count),
            static (name, _, count) => new IndexProgress(name, IndexState.Building, count),
            itemCount);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetCompleted(string commandName, int itemCount)
        => _progress.AddOrUpdate(
            commandName,
            static (name, count) => new IndexProgress(name, IndexState.Completed, count),
            static (name, _, count) => new IndexProgress(name, IndexState.Completed, count),
            itemCount);

    public bool IsAllCompleted
    {
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        get
        {
            if (_progress.IsEmpty) return false;

            foreach (var kvp in _progress)
                if (kvp.Value.State != IndexState.Completed)
                    return false;

            return true;
        }
    }

    public IEnumerable<IndexProgress> GetProgress() => _progress.Values.OrderBy(static p => p.State);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public void SetTrieTelemetry(TrieIndexTelemetry telemetry)
        => _trieTelemetry[telemetry.CommandName] = telemetry;

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public bool TryGetTrieTelemetry(string commandName, out TrieIndexTelemetry telemetry)
        => _trieTelemetry.TryGetValue(commandName, out telemetry);

    public void IncrementTrieRetrieveException(string commandName)
        => _trieTelemetry.AddOrUpdate(
            commandName,
            static name => new TrieIndexTelemetry(
                name,
                TotalIndices: 0,
                DescriptionIndices: 0,
                NormalizedNull: 0,
                NormalizedTooShort: 0,
                NormalizedTooLong: 0,
                DuplicateKeys: 0,
                AddedKeys: 0,
                AddArgumentExceptions: 0,
                AddOutOfRangeExceptions: 0,
                AddNullReferenceExceptions: 0,
                RetrieveExceptions: 1,
                BuildElapsedMs: 0,
                TrieEnabled: false),
            static (_, existing) => existing with { RetrieveExceptions = existing.RetrieveExceptions + 1 });

    public string GetProgressMessage()
    {
        var snapshot = _progress.Values.ToArray();
        if (snapshot.Length == 0)
            return "No commands registered for indexing";

        Array.Sort(snapshot, static (a, b) => GetDisplayPriority(a.State).CompareTo(GetDisplayPriority(b.State)));

        var completedCount = 0;
        var totalItems = 0;
        foreach (var p in snapshot)
        {
            if (p.State == IndexState.Completed)
            {
                completedCount++;
                totalItems += p.ItemCount;
            }
        }

        var sb = new StringBuilder(256);
        sb.Append("Indexing progress (").Append(completedCount).Append('/').Append(snapshot.Length).Append("): ");

        IndexState? currentState = null;
        var isFirstInGroup = true;
        var isFirstGroup = true;

        foreach (var p in snapshot)
        {
            if (currentState != p.State)
            {
                if (currentState.HasValue)
                    AppendGroupSuffix(sb, currentState.Value, totalItems);

                if (!isFirstGroup) sb.Append(" | ");
                isFirstGroup = false;

                AppendGroupPrefix(sb, p.State);
                currentState = p.State;
                isFirstInGroup = true;
            }

            if (!isFirstInGroup) sb.Append(", ");
            isFirstInGroup = false;

            sb.Append(p.CommandName);

            if (p.State == IndexState.Building && p.ItemCount > 0)
                sb.Append(" (").Append(p.ItemCount.ToString("N0")).Append(')');
        }

        if (currentState.HasValue)
            AppendGroupSuffix(sb, currentState.Value, totalItems);

        return sb.ToString();
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static int GetDisplayPriority(IndexState state) => state switch
    {
        IndexState.Completed => 0,
        IndexState.Building => 1,
        IndexState.Loading => 2,
        IndexState.Pending => 3,
        _ => 99
    };

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendGroupPrefix(StringBuilder sb, IndexState state)
    {
        sb.Append(state switch
        {
            IndexState.Completed => "#g",  // Green
            IndexState.Building => "#e",   // Bold/emphasis
            IndexState.Loading => "#b",    // Blue
            IndexState.Pending => "#d",    // Dark/gray
            _ => ""
        });
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void AppendGroupSuffix(StringBuilder sb, IndexState state, int totalCompletedItems)
    {
        sb.Append(state switch
        {
            IndexState.Completed => $"#k ({totalCompletedItems:N0} items)",
            IndexState.Building => "#n (building trie...)",
            IndexState.Loading => "#k (loading data...)",
            IndexState.Pending => "#k (pending)",
            _ => ""
        });
    }
}
