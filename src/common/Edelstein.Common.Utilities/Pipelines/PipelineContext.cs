using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class PipelineContext : IPipelineContext
{
    public bool IsRequestedCancellation { get; private set; }
    public bool IsRequestedDefaultAction { get; private set; }

    public void Default() => IsRequestedDefaultAction = true;
    public void Cancel() => IsRequestedCancellation = true;
}
