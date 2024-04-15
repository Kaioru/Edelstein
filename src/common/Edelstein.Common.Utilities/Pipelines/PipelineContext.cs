using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Utilities.Pipelines;

public class PipelineContext : IPipelineContext
{
    public bool IsRequestedCancellation { get; private set; }
    
    public void Cancel() => IsRequestedCancellation = true;
}
