﻿using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages.Login
{
    public class LoginStageUser : AbstractMigrateableStageUser<LoginStage, LoginStageUser, LoginStageConfig>, ILoginStageUser<LoginStage, LoginStageUser>
    {
        public LoginStageUser(ISocket socket) : base(socket) { }
    }
}
