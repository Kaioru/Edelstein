using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Messages;

public record UserTransferChannelRequest(
    IFieldUser User,
    int ChannelID
) : IUserTransferChannelRequest;
