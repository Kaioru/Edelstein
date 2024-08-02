namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipelineContext
{
    bool IsRequestedSkipToDefault { get; }
    bool IsRequestedCancellation { get; }
    
    void SkipToDefault();
    void Cancel();
}
