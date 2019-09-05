using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core;
using Edelstein.Core.Distributed.Migrations;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Database.Entities;
using Edelstein.Database.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Trade.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Trade.Services
{
    public partial class TradeSocket : AbstractMigrateableSocket<TradeServiceInfo>
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public TradeService Service { get; }

        public SocialServiceInfo SocialService => Service.Peers
            .OfType<SocialServiceInfo>()
            .FirstOrDefault(s => s.Worlds.Contains(AccountData.WorldID));

        public Account Account { get; set; }
        public AccountData AccountData { get; set; }
        public Character Character { get; set; }

        public TradeSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            TradeService service
        ) : base(channel, seqSend, seqRecv, service)
        {
            Service = service;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();
            return operation switch {
                RecvPacketOperations.MigrateIn => OnMigrateIn(packet),
                RecvPacketOperations.AliveAck => TryProcessHeartbeat(Account, Character),
                RecvPacketOperations.UserTransferFieldRequest => OnUserTransferFieldRequest(packet),
                _ => Task.Run(() => Logger.Warn($"Unhandled packet operation {operation}"))
                };
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Socket caught exception");
            return Task.CompletedTask;
        }

        public override async Task OnUpdate()
        {
            using (var store = Service.DataStore.OpenSession())
            {
                await store.UpdateAsync(Account);
                await store.UpdateAsync(AccountData);
                await store.UpdateAsync(Character);
            }
        }

        public override async Task OnDisconnect()
        {
            if (Account == null) return;

            var state = (await Service.AccountStateCache.GetAsync<MigrationState>(Account.ID.ToString())).Value;

            if (state != MigrationState.Migrating)
            {
                await OnUpdate();
                await Service.AccountStateCache.RemoveAsync(Account.ID.ToString());
                
                if (SocialService != null)
                    await Service.SendMessage(SocialService, new SocialUpdateStateMessage
                    {
                        CharacterID = Character.ID,
                        State = MigrationState.LoggedOut,
                        Service = Service.Info.Name
                    });
            }
        }
    }
}