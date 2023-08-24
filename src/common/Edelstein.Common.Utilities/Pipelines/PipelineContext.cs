using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class PipelineContext : IPipelineContext
{
    public bool IsRequestedDefaultAction { get; private set; }
    public bool IsRequestedCancellation { get; private set; }
    
    public void Cancel() => IsRequestedCancellation = true;

    public void Default() => IsRequestedDefaultAction = true;
}
