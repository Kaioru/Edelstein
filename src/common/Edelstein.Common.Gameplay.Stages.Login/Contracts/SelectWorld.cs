using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts;

public record SelectWorld(
    ILoginStageUser User,
    int WorldID,
    int ChannelID
) : ISelectWorld;
