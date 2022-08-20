using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record SelectWorld(
    ILoginStageUser User,
    int WorldID,
    int ChannelID
) : ISelectWorld;
