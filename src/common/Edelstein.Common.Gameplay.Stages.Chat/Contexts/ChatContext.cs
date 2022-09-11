using Edelstein.Protocol.Gameplay.Stages.Chat.Contexts;

namespace Edelstein.Common.Gameplay.Stages.Chat.Contexts;

public record ChatContext(
    IChatContextPipelines Pipelines
) : IChatContext;
