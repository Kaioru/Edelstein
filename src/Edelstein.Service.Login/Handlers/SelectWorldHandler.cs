using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class SelectWorldHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            packet.Decode<byte>();

            var worldID = packet.Decode<byte>();
            var channelID = packet.Decode<byte>();

            if (adapter.Account == null) return;

            try
            {
                using var p = new Packet(SendPacketOperations.SelectWorldResult);
                using var store = adapter.Service.DataStore.StartSession();
                var result = LoginResultCode.Success;

                var service = (await adapter.Service.GetPeers())
                    .Select(n => n.State)
                    .OfType<GameServiceState>()
                    .FirstOrDefault(s =>
                        s.ChannelID == channelID &&
                        s.Worlds.Contains(worldID)
                    );

                if (service == null) result = LoginResultCode.NotConnectableWorld;

                p.Encode<byte>((byte) result);

                if (result == LoginResultCode.Success)
                {
                    var accountWorld = store
                        .Query<AccountWorld>()
                        .FirstOrDefault(a =>
                            a.AccountID == adapter.Account.ID &&
                            a.WorldID == worldID
                        );

                    if (accountWorld == null)
                    {
                        accountWorld = new AccountWorld
                        {
                            AccountID = adapter.Account.ID,
                            WorldID = worldID
                        };
                        await store.InsertAsync(accountWorld);
                    }

                    adapter.AccountWorld = accountWorld;
                    if (adapter.Account.LatestConnectedWorld != worldID)
                        adapter.Account.LatestConnectedWorld = worldID;

                    p.Encode<byte>(0); // TODO characters
                    p.Encode<bool>(
                        !string.IsNullOrEmpty(adapter.Account.PIC)
                    ); // bLoginOpt TODO: proper bLoginOpt stuff
                    p.Encode<int>(3); // TODO: nSlotCount
                    p.Encode<int>(0);
                }

                await adapter.SendPacket(p);
            }
            catch
            {
                using var p = new Packet(SendPacketOperations.SelectWorldResult);

                p.Encode<byte>((byte) LoginResultCode.Unknown);

                await adapter.SendPacket(p);
            }
        }
    }
}