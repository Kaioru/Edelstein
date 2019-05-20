namespace Edelstein.Service.Game.Interactions
{
    public enum ShopResult
    {
        BuySuccess = 0x0,
        BuyNoStock = 0x1,
        BuyNoMoney = 0x2,
        BuyUnknown = 0x3,
        SellSuccess = 0x4,
        SellNoStock = 0x5,
        SellIncorrectRequest = 0x6,
        SellUnkonwn = 0x7,
        RechargeSuccess = 0x8,
        RechargeNoStock = 0x9,
        RechargeNoMoney = 0xA,
        RechargeIncorrectRequest = 0xB,
        RechargeUnknown = 0xC,
        BuyNoToken = 0xD,
        LimitLevel_Less = 0xE,
        LimitLevel_More = 0xF,
        CantBuyAnymore = 0x10,
        TradeBlocked = 0x11,
        BuyLimit = 0x12,
        ServerMsg = 0x13
    }
}