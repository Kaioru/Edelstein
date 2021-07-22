using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStageUser<TStage, TUser> : ISession
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        Account Account { get; set; }
        AccountWorld AccountWorld { get; set; }
        Character Character { get; set; }

        TStage Stage { get; set; }
    }
}
