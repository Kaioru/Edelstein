namespace Edelstein.Protocol.Gameplay.Stages.CashShop
{
    public interface ICashShopStage<TStage, TUser> : IServerStage<TStage, TUser>
        where TStage : ICashShopStage<TStage, TUser>
        where TUser : ICashShopStageUser<TStage, TUser>
    {
    }
}
