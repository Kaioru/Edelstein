﻿using Edelstein.Common.Gameplay.Stages.Messages;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Gameplay.Stages.Login.Contexts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Stages.Login;

public class LoginStageUser : AbstractStageUser<ILoginStageUser>, ILoginStageUser
{
    public LoginStageUser(
        ISocket socket,
        ILoginContext context
    ) : base(socket) =>
        Context = context;

    public ILoginContext Context { get; }

    public LoginState State { get; set; }
    public byte? SelectedWorldID { get; set; }
    public byte? SelectedChannelID { get; set; }

    public override Task OnPacket(IPacketReader packet) =>
        Context.Pipelines.SocketOnPacket.Process(new SocketOnPacket<ILoginStageUser>(this, packet));

    public override Task OnException(Exception exception) =>
        Context.Pipelines.SocketOnException.Process(new SocketOnException<ILoginStageUser>(this, exception));

    public override Task OnDisconnect() =>
        Context.Pipelines.SocketOnDisconnect.Process(new SocketOnDisconnect<ILoginStageUser>(this));
}