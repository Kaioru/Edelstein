﻿namespace Edelstein.Protocol.Gameplay.Stages.Login;

public interface ILoginStageUser : IStageUser<ILoginStage, ILoginStageUser>
{
    LoginState State { get; set; }

    byte? SelectedWorldID { get; set; }
    byte? SelectedChannelID { get; set; }
}
