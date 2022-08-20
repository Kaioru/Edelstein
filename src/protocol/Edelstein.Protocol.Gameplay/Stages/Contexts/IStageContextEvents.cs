using Edelstein.Protocol.Gameplay.Stages.Contracts.Events;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Protocol.Gameplay.Stages.Contexts;

public interface IStageContextEvents<TStageUser> where TStageUser : IStageUser<TStageUser>
{
    IEvent<IUserEnterStage<TStageUser>> UserEnterStage { get; }
    IEvent<IUserLeaveStage<TStageUser>> UserLeaveStage { get; }
}
