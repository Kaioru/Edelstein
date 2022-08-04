﻿using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Messages;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Messages;

public record UserTransferChannelRequest(
    IFieldUser User,
    int ChannelID
) : IUserTransferChannelRequest;
