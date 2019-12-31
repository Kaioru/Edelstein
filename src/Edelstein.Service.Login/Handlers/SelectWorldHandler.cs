using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Extensions.Packets;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;
using MoreLinq;

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

                var peers = await adapter.Service.GetPeers();
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
                        .Where(a =>
                            a.AccountID == adapter.Account.ID &&
                            a.WorldID == worldID
                        )
                        .FirstOrDefault();

                    if (accountWorld == null)
                    {
                        accountWorld = new AccountWorld
                        {
                            AccountID = adapter.Account.ID,
                            WorldID = worldID
                        };
                        await store.InsertAsync(accountWorld);
                    }

                    adapter.SelectedNode = service;
                    adapter.AccountWorld = accountWorld;
                    if (adapter.Account.LatestConnectedWorld != worldID)
                        adapter.Account.LatestConnectedWorld = worldID;

                    var characters = store
                        .Query<Character>()
                        .Where(c => c.AccountWorldID == accountWorld.ID)
                        .ToImmutableList();

                    p.Encode<byte>((byte) characters.Count);
                    characters.ForEach(c =>
                    {
                        c.EncodeStats(p);
                        c.EncodeLook(p);

                        p.Encode<bool>(false);
                        p.Encode<bool>(false);
                    });

                    p.Encode<bool>(
                        !string.IsNullOrEmpty(adapter.Account.SPW)
                    ); // bLoginOpt TODO: proper bLoginOpt stuff
                    p.Encode<int>(accountWorld.SlotCount);
                    p.Encode<int>(0);
                }

                await adapter.SendPacket(p);
            }
            catch
            {
                using var p = new Packet(SendPacketOperations.SelectWorldResult);

                p.Encode<byte>((byte) LoginResultCode.DBFail);

                await adapter.SendPacket(p);
            }
        }
    }
}