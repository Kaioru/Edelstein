using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Interop;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStage : IMigrateableStage<IGameStage, IGameStageUser>
    {
        int WorldID { get; }
        int ChannelID { get; }

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
