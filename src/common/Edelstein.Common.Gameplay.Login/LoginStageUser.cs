﻿using Edelstein.Protocol.Gameplay.Contracts;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Login;

public class LoginStageUser : AbstractStageUser<ILoginStageUser>, ILoginStageUser
{

    public LoginStageUser(
        ISocket socket,
        LoginContext context
    ) : base(socket) =>
        Context = context;
    public LoginContext Context { get; }

    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public override Task Migrate(string serverID, IPacket? packet = null)
        => Context.Pipelines.UserMigrate.Process(new UserMigrate<ILoginStageUser>(this, serverID, packet));

    public override Task OnPacket(IPacket packet)
        => Context.Pipelines.UserOnPacket.Process(new UserOnPacket<ILoginStageUser>(this, packet));

    public override Task OnException(Exception exception)
        => Context.Pipelines.UserOnException.Process(new UserOnException<ILoginStageUser>(this, exception));

    public override Task OnDisconnect()
        => Context.Pipelines.UserOnDisconnect.Process(new UserOnDisconnect<ILoginStageUser>(this));
}
