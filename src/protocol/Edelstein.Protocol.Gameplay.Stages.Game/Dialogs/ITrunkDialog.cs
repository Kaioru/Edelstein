using Edelstein.Protocol.Gameplay.Users.Inventories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs
{
    public interface ITrunkDialog : IDialog
    {
        int TemplateID { get; }
        ItemTrunk Trunk { get; }
    }
}
