using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Chat;

public interface IChatStageUser : IStageUser<IChatStageUser>
{
    IChatContext Context { get; }
}
