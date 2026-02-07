namespace Edelstein.Plugin.Rue.Commands;

/// <summary>
/// A command that supports pre-indexing for faster search operations.
/// </summary>
public interface IIndexedCommand : ICommand
{
    Task Index(IndexingStatus status);
}
