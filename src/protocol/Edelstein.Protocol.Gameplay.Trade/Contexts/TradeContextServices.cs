﻿using Edelstein.Protocol.Services.Auth;
using Edelstein.Protocol.Services.Migration;
using Edelstein.Protocol.Services.Server;
using Edelstein.Protocol.Services.Session;

namespace Edelstein.Protocol.Gameplay.Trade.Contexts;

public record TradeContextServices(
    IAuthService Auth,
    IServerService Server,
    ISessionService Session,
    IMigrationService Migration
);
