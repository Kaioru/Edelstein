namespace Edelstein.Service.Game.Interactions
{
    public enum ShopRequest : byte
    {
        Buy = 0x0,
        Sell = 0x1,
        Recharge = 0x2,
        Close = 0x3
    }
}