using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;

public record UserTransferChannelRequest(
    IFieldUser User,
    int ChannelID
) : IUserTransferChannelRequest;
