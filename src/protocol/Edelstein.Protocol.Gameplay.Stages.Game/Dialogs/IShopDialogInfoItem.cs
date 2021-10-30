namespace Edelstein.Protocol.Gameplay.Stages.Game.Dialogs
{
    public interface IShopDialogInfoItem
    {
        int TemplateID { get; }

        int Price { get; }
        byte DiscountRate { get; }

        int TokenTemplateID { get; }
        int TokenPrice { get; }

        int ItemPeriod { get; }
        int LevelLimited { get; }
        double UnitPrice { get; }
        short MaxPerSlot { get; }
        int Quantity { get; }
    }
}
