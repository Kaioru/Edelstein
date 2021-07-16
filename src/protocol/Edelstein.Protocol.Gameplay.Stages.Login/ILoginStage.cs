using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Login
{ 
    public interface ILoginStage : IStage<ILoginStage, ILoginStageUser>, IRepositoryEntry<int>
    {
        IServerRegistryService ServerRegistryService { get; }
        ISessionRegistryService SessionRegistry { get; }
        IMigrationRegistryService MigrationRegistryService { get; }

        IAccountRepository AccountRepository { get; }
        IAccountWorldRepository AccountWorldRepository { get; }
        ICharacterRepository CharacterRepository { get; }

        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
    }
}
