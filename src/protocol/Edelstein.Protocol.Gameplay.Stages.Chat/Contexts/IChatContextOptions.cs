using Edelstein.Protocol.Gameplay.Stages.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;

public interface IChatContextOptions : IStageContextOptions
{
    int WorldID { get; }
}
