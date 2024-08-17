using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Templates;

public interface ITemplateManagerLoader<TTemplate>
    where TTemplate : ITemplate
{
    Task Load(ITemplateManagerContext<TTemplate> context);
}
