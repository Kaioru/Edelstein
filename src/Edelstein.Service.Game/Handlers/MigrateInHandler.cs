using System.Threading.Tasks;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Utils;
using Edelstein.Core.Utils.Packets;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Handlers
{
    public class MigrateInHandler : AbstractPacketHandler<GameServiceAdapter>
    {
        protected override async Task Handle(
            GameServiceAdapter adapter,
            RecvPacketOperations operation,
            IPacket packet
        )
        {
            var characterID = packet.Decode<int>();

            packet.Decode<long>(); // MachineID 1
            packet.Decode<long>(); // MachineID 2

            packet.Decode<bool>(); // isUserGM
            packet.Decode<byte>(); // Unk

            var clientKey = packet.Decode<long>();

            try
            {
                await adapter.TryMigrateFrom(characterID, clientKey);

                var field = adapter.Service.FieldManager.Get(adapter.Character.FieldID);
                var fieldUser = new FieldUser(adapter)
                {
                    Party = await adapter.Service.PartyManager.Load(adapter.Character),
                    Guild = await adapter.Service.GuildManager.Load(adapter.Character)
                };

                (await adapter.Service.MemoManager.Load(adapter.Character))
                    .ForEach(m => fieldUser.Memos.Add(m.ID, m));

                adapter.User = fieldUser;

                await fieldUser.UpdateStats();
                await field.Enter(fieldUser);

                if (fieldUser.Memos.Count > 0)
                    using (var p = new Packet(SendPacketOperations.MemoResult))
                    {
                        var memos = fieldUser.Memos.Values;

                        p.Encode<byte>((byte) MemoResultType.Load);
                        p.Encode<byte>((byte) memos.Count);
                        memos.ForEach(m => m.EncodeData(p));
                        await fieldUser.SendPacket(p);
                    }
            }
            catch
            {
                await adapter.Close();
            }
        }
    }
}