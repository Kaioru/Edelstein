using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Characters;
using Edelstein.Core.Network.Packets;
using Edelstein.Core.Utils.Packets;
using Edelstein.Service.Login.Types;

namespace Edelstein.Service.Login.Handlers
{
    public class DeleteCharacterHandler : AbstractPacketHandler<LoginServiceAdapter>
    {
        protected override async Task Handle(
            LoginServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacketDecoder packet
        )
        {
            var spw = packet.DecodeString();
            var characterID = packet.DecodeInt();
            try
            {
                using var p = new OutPacket(SendPacketOperations.DeleteCharacterResult);
                using var store = adapter.Service.DataStore.StartSession();
                var character = store.Query<Character>()
                    .Where(c =>
                        c.AccountWorldID == adapter.AccountWorld.ID &&
                        c.ID == characterID
                    )
                    .First();
                var result = LoginResultCode.Success;

                if (!BCrypt.Net.BCrypt.Verify(spw, adapter.Account.SPW))
                    result = LoginResultCode.IncorrectSPW;

                var party = await adapter.Service.PartyManager.Load(character);
                var guild = await adapter.Service.GuildManager.Load(character);
                var partyMember = party?.Members
                    .FirstOrDefault(m => m.CharacterID == characterID);
                var guildMember = guild?.Members
                    .FirstOrDefault(m => m.CharacterID == characterID);

                if (guildMember?.Grade == 1)
                    result = LoginResultCode.DeleteCharacterFailedOnGuildMaster;

                p.EncodeInt(characterID);
                p.EncodeByte((byte) result);

                if (result == LoginResultCode.Success)
                {
                    await store.DeleteAsync(character);
                    if (partyMember != null) await partyMember.Withdraw();
                    if (guildMember != null) await guildMember.Withdraw();
                }

                await adapter.SendPacket(p);
            }
            catch

            {
                using var p = new OutPacket(SendPacketOperations.DeleteCharacterResult);

                p.EncodeInt(characterID);
                p.EncodeByte((byte) LoginResultCode.Unknown);

                await adapter.SendPacket(p);
            }
        }
    }
}