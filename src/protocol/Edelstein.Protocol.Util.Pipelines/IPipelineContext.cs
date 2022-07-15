namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipelineContext
{
    bool IsRequestedCancellation { get; }

    void Cancel();
}
