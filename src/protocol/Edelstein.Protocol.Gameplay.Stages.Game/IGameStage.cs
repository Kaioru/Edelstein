using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStage : IStage<IGameStage, IGameStageUser>, IRepositoryEntry<int>
    {
        int WorldID { get; }

        IServerRegistryService ServerRegistryService { get; }
        ISessionRegistryService SessionRegistry { get; }
        IMigrationRegistryService MigrationRegistryService { get; }

        IAccountRepository AccountRepository { get; }
        IAccountWorldRepository AccountWorldRepository { get; }
        ICharacterRepository CharacterRepository { get; }

        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        ITemplateRepository<FieldTemplate> FieldTemplates { get; }

        IFieldRepository FieldRepository { get; }
        IFieldSetRepository FieldSetRepository { get; }
    }
}
