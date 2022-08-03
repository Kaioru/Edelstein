using Edelstein.Protocol.Gameplay.Stages.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextPipelines : IStageContextPipelines<IGameStageUser>
{
    IPipeline<IUserMove> UserMove { get; }
}
