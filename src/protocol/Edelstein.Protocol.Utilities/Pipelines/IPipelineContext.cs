namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipelineContext
{
    bool IsRequestedCancellation { get; }

    void Cancel();
}
