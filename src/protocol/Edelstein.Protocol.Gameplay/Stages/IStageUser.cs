using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network.Session;

namespace Edelstein.Protocol.Gameplay.Stages
{
    public interface IStageUser<TStage, TUser> : ISession
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        int ID { get; }

        Account Account { get; }
        AccountWorld AccountWorld { get; }
        Character Character { get; }

        TStage Stage { get; }

        Task Update();
    }
}
