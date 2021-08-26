using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Options;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates.Sets;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStage<TStage, TUser> : IServerStage<TStage, TUser>
        where TStage : IGameStage<TStage, TUser>
        where TUser : IGameStageUser<TStage, TUser>
    {
        int WorldID { get; }
        int ChannelID { get; }

        IDispatchService DispatchService { get; }
        IGuildService GuildService { get; }
        IPartyService PartyService { get; }

        ICommandProcessor CommandProcessor { get; }

        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        ITemplateRepository<ItemOptionTemplate> ItemOptionTemplates { get; }
        ITemplateRepository<ItemSetTemplate> ItemSetTemplates { get; }

        ITemplateRepository<FieldTemplate> FieldTemplates { get; }
        ITemplateRepository<NPCTemplate> NPCTemplates { get; }

        IFieldRepository FieldRepository { get; }
        IFieldSetRepository FieldSetRepository { get; }
        IContiMoveRepository ContiMoveRepository { get; }
    }
}
