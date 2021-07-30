using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Gameplay.Users.Inventories.Templates;

namespace Edelstein.Protocol.Gameplay.Stages.Login
{
    public interface ILoginStage<TStage, TUser> : IMigrateableStage<TStage, TUser>
        where TStage : ILoginStage<TStage, TUser>
        where TUser : ILoginStageUser<TStage, TUser>
    {
        ITemplateRepository<ItemTemplate> ItemTemplates { get; }
    }
}
