using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class PipelineContext : IPipelineContext
{
    public bool IsRequestedSkipToDefault { get; private set; }
    public bool IsRequestedCancellation { get; private set; }

    public void SkipToDefault() => IsRequestedSkipToDefault = true;
    public void Cancel() => IsRequestedCancellation = true;
}
