using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Entities;
using Edelstein.Entities.Characters;
using Edelstein.Network.Packets;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class DeleteCharacterHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var spw = packet.Decode<string>();
            var characterID = packet.Decode<int>();
            try
            {
                using var p = new Packet(SendPacketOperations.DeleteCharacterResult);
                using var store = adapter.Service.DataStore.StartSession();

                var result = LoginResultCode.Success;

                if (!BCrypt.Net.BCrypt.Verify(spw, adapter.Account.SPW))
                    result = LoginResultCode.IncorrectSPW;

                p.Encode<int>(characterID);
                p.Encode<byte>((byte) result);

                if (result == LoginResultCode.Success)
                {
                    var character = store.Query<Character>()
                        .Where(c =>
                            c.AccountWorldID == adapter.AccountWorld.ID &&
                            c.ID == characterID
                        )
                        .First();
                    await store.DeleteAsync(character);
                }

                await adapter.SendPacket(p);
            }
            catch
            {
                using var p = new Packet(SendPacketOperations.DeleteCharacterResult);

                p.Encode<int>(characterID);
                p.Encode<byte>((byte) LoginResultCode.Unknown);

                await adapter.SendPacket(p);
            }
        }
    }
}