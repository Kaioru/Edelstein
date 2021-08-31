using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Templating
{
    public interface ITemplateRepository<
        TEntry
    > : ILocalRepositoryReader<int, TEntry>
        where TEntry : class, ITemplate
    {
    }
}
