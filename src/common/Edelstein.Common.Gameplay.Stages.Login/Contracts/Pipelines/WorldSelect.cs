using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Contracts.Pipelines;

public record WorldSelect(
    ILoginStageUser User,
    byte WorldID,
    byte ChannelID
) : IWorldSelect;
