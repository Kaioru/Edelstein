namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipelineAction<in TMessage>
{
    Task Handle(IPipelineContext ctx, TMessage message);
}
