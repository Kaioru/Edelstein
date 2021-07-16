using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStageUser<TStage, TUser> : ISession
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        Account Account { get; init; }
        AccountWorld AccountWorld { get; }
        Character Character { get; }

        TStage Stage { get; set; }
    }
}
