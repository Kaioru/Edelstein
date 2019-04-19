using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Services
{
    public class GameSocket : AbstractMigrateableSocket<GameServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public GameService Service { get; }

        public GameSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            GameService service
        ) : base(channel, seqSend, seqRecv, service)
        {
            Service = service;
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