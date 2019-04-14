using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Database;
using Edelstein.Network;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Sockets
{
    public class LoginSocket : AbstractMigrateableSocket<LoginServiceInfo>
    {
        public WvsLogin WvsLogin { get; }

        public LoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsLogin service
        ) : base(channel, seqSend, seqRecv, service)
        {
            WvsLogin = service;
        }

        public override Task OnPacket(IPacket packet)
        {
            throw new NotImplementedException();
        }

        public override Task OnException(Exception exception)
        {
            throw new NotImplementedException();
        }

        public override Task OnDisconnect()
        {
            throw new NotImplementedException();
        }
    }
}