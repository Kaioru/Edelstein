using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Distributed.States;
using Edelstein.Core.Gameplay.Migrations.States;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckSPWRequestHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        private readonly bool _vac;

        public CheckSPWRequestHandler(bool vac)
            => _vac = vac;

        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var spw = packet.DecodeString();
            var characterID = packet.DecodeInt();
            packet.DecodeString(); // sMacAddress
            packet.DecodeString(); // sMacAddressWithHDDSerial

            try
            {
                using var store = adapter.Service.DataStore.StartSession();
                var result = LoginResultCode.Success;

                if (_vac)
                {
                    adapter.AccountWorld = store.Query<AccountWorld>()
                        .Where(a => a.AccountID == adapter.Account.ID)
                        .Where(a => a.ID == characterID)
                        .First();
                    adapter.SelectedNode = (await adapter.Service.GetPeers())
                        .Select(n => n.State)
                        .OfType<GameNodeState>()
                        .First(s => s.Worlds.Contains(adapter.AccountWorld.WorldID));

                    if (adapter.Service.State.Worlds.All(w => w.ID != adapter.AccountWorld.WorldID))
                        result = LoginResultCode.NotConnectableWorld;
                }

                if (string.IsNullOrEmpty(adapter.Account.SPW))
                    result = LoginResultCode.Unknown;
                else if (!BCrypt.Net.BCrypt.Verify(spw, adapter.Account.SPW))
                    result = LoginResultCode.IncorrectSPW;

                var character = store.Query<Character>()
                    .Where(c => c.AccountWorldID == adapter.AccountWorld.ID)
                    .Where(c => c.ID == characterID)
                    .First();

                if (result != LoginResultCode.Success)
                {
                    using var p = new OutPacket(SendPacketOperations.EnableSPWResult);

                    p.EncodeBool(false);
                    p.EncodeByte((byte) result);

                    await adapter.SendPacket(p);
                    return;
                }

                adapter.Character = character;
                await adapter.TryMigrateTo(adapter.SelectedNode);
            }
            catch
            {
                using var p = new OutPacket(SendPacketOperations.CheckSPWResult);

                p.EncodeByte((byte) LoginResultCode.Unknown);

                await adapter.SendPacket(p);
            }
        }
    }
}