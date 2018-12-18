using System;
using System.Net;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Logging;
using Foundatio.Caching;
using MoreLinq;

namespace Edelstein.Service.Login.Sockets
{
    public partial class WvsLoginSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();
        public WvsLogin WvsLogin { get; }
        public Account Account { get; set; }
        public GameServiceInfo SelectedService { get; set; }
        public Character SelectedCharacter { get; set; }

        public WvsLoginSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsLogin wvsLogin
        ) : base(channel, seqSend, seqRecv)
        {
            WvsLogin = wvsLogin;
        }

        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.CheckPassword:
                    return OnCheckPassword(packet);
                case RecvPacketOperations.WorldInfoRequest:
                case RecvPacketOperations.WorldRequest:
                    return OnWorldInfoRequest(packet);
                case RecvPacketOperations.SelectWorld:
                    return OnSelectWorld(packet);
                case RecvPacketOperations.CheckUserLimit:
                    return OnCheckUserLimit(packet);
                case RecvPacketOperations.CheckDuplicatedID:
                    return OnCheckDuplicatedID(packet);
                case RecvPacketOperations.CreateNewCharacter:
                    return OnCreateNewCharacter(packet);
                case RecvPacketOperations.EnableSPWRequest:
                    return OnEnableSPWRequest(packet, false);
                case RecvPacketOperations.CheckSPWRequest:
                    return OnCheckSPWRequest(packet, false);
                case RecvPacketOperations.EnableSPWRequestByACV:
                    return OnEnableSPWRequest(packet, true);
                case RecvPacketOperations.CheckSPWRequestByACV:
                    return OnCheckSPWRequest(packet, true);
                default:
                    Logger.Warn($"Unhandled packet operation {operation}");
                    return Task.CompletedTask;
            }
        }

        public override async Task OnDisconnect()
        {
            if (Account != null)
            {
                var state = await WvsLogin.AccountStatusCache.GetAsync<AccountState>(Account.ID.ToString());

                if (state.HasValue && state.Value != AccountState.MigratingIn)
                    await WvsLogin.AccountStatusCache.RemoveAsync(Account.ID.ToString());
            }
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Caught exception in socket handling");
            return Task.CompletedTask;
        }

        public async Task<bool> Migrate(ServerServiceInfo info)
        {
            if (SelectedCharacter == null) return false;
            if (!await WvsLogin.TryMigrateTo(SelectedCharacter.ID, info)) return false;

            using (var p = new Packet(SendPacketOperations.SelectCharacterResult))
            {
                p.Encode<byte>(0);
                p.Encode<byte>(0);

                var endpoint = new IPEndPoint(IPAddress.Parse(info.Host), info.Port);
                var address = endpoint.Address.MapToIPv4().GetAddressBytes();
                var port = endpoint.Port;

                address.ForEach(b => p.Encode<byte>(b));
                p.Encode<short>((short) port);

                p.Encode<int>(SelectedCharacter.ID);
                p.Encode<byte>(0);
                p.Encode<int>(0);

                await SendPacket(p);
            }

            return true;
        }
    }
}