using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Util.Pipelines;

public class PipelineContext : IPipelineContext
{
    public bool IsRequestedCancellation { get; private set; }
    public bool IsRequestedCancellationDefault { get; private set; }

    public void Cancel() => IsRequestedCancellation = true;
    public void CancelDefault() => IsRequestedCancellationDefault = true;
}
