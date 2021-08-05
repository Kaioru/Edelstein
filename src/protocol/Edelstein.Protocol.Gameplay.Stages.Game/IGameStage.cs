using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.FieldSets;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Game
{
    public interface IGameStage<TStage, TUser> : IServerStage<TStage, TUser>
        where TStage : IGameStage<TStage, TUser>
        where TUser : IGameStageUser<TStage, TUser>
    {
        int WorldID { get; }
        int ChannelID { get; }

        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
        ITemplateRepository<FieldTemplate> FieldTemplates { get; }

        IFieldRepository FieldRepository { get; }
        IFieldSetRepository FieldSetRepository { get; }
        IContiMoveRepository ContiMoveRepository { get; }
    }
}
