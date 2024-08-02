using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Pipelines;

public interface IPipe<in TMessage>
{
    Task Handle(IPipelineContext ctx, TMessage message);
}
