namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipelineContext
{
    bool IsRequestedCancellation { get; }
    bool IsRequestedCancellationDefault { get; }

    void Cancel();
    void CancelDefault();
}
