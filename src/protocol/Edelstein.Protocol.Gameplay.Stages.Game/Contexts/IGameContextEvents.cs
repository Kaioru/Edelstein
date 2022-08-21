using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Contracts.Events;
using Edelstein.Protocol.Util.Events;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextEvents : IStageContextEvents<IGameStageUser>
{
    IEvent<IObjectEnterField> ObjectEnterField { get; }
    IEvent<IObjectLeaveField> ObjectLeaveField { get; }
    IEvent<IObjectEnterFieldSplit> ObjectEnterFieldSplit { get; }
    IEvent<IObjectLeaveFieldSplit> ObjectLeaveFieldSplit { get; }
}
