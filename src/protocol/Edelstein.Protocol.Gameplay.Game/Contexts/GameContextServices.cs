﻿using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Social;

namespace Edelstein.Protocol.Gameplay.Game.Contexts;

public record GameContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration,
    
    IFriendService Friend,
    IPartyService Party
);
