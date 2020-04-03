using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;

namespace Edelstein.Service.Login.Handlers
{
    public class CheckDuplicatedIDHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var name = packet.DecodeString();

            if (adapter.Account == null) return;
            if (adapter.AccountWorld == null) return;

            var token = new CancellationTokenSource();

            token.CancelAfter(TimeSpan.FromSeconds(5));

            var createLock =
                await adapter.Service.LockProvider.AcquireAsync(
                    LoginService.AuthLockKey,
                    cancellationToken: token.Token
                );

            if (createLock == null)
            {
                using var p = new Packet(SendPacketOperations.CheckDuplicatedIDResult);

                p.EncodeString(name);
                p.EncodeByte(0x2);

                await adapter.SendPacket(p);
                return;
            }

            try
            {
                using var p = new Packet(SendPacketOperations.CheckDuplicatedIDResult);
                using var store = adapter.Service.DataStore.StartSession();

                var isDuplicatedID = store
                    .Query<Character>()
                    .Where(c => c.Name == name)
                    .ToImmutableList()
                    .Any();

                p.EncodeString(name);
                p.EncodeBool(isDuplicatedID);

                await adapter.SendPacket(p);
            }
            finally
            {
                await createLock.ReleaseAsync();
            }
        }
    }
}