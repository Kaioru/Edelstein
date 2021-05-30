using System.Threading.Tasks;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Templating
{
    public interface ITemplateRepository<
        TEntry
    > : ILocalRepositoryReader<int, TEntry>,
        ILocalRepositoryWriter<int, TEntry>
        where TEntry : class, ITemplate
    {
        Task Populate();
    }
}
