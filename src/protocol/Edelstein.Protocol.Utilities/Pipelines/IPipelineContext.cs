namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipelineContext
{
    bool IsRequestedCancellation { get; }
    bool IsRequestedDefaultAction { get; }

    void Default();
    void Cancel();
}
