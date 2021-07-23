using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Login
{
    public interface ILoginStage : IMigrateableStage<ILoginStage, ILoginStageUser>, IRepositoryEntry<int>
    {
        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
    }
}
