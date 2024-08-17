using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Utilities.Templates;

public class TemplateManagerContext<TTemplate> :
    Repository<int, ITemplateProvider<TTemplate>>,
    ITemplateManagerContext<TTemplate>
    where TTemplate : ITemplate;
