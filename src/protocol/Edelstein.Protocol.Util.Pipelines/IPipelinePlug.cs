namespace Edelstein.Protocol.Util.Pipelines;

public interface IPipelinePlug<in TMessage>
{
    Task Handle(IPipelineContext ctx, TMessage message);
}
