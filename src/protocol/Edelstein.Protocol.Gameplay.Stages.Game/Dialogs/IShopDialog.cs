namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs
{
    public interface IShopDialog : IDialog
    {
        int TemplateID { get; }
        IShopDialogInfo Info { get; }
    }
}
