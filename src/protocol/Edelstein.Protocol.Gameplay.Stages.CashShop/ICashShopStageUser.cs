namespace Edelstein.Protocol.Gameplay.Stages.CashShop
{
    public interface ICashShopStageUser<TStage, TUser> : IServerStageUser<TStage, TUser>
        where TStage : ICashShopStage<TStage, TUser>
        where TUser : ICashShopStageUser<TStage, TUser>
    {
    }
}
