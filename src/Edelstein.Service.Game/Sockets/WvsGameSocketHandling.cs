using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Types;
using Edelstein.Network.Packet;
using Edelstein.Service.Game.Fields.User;
using Microsoft.EntityFrameworkCore;
using MoreLinq;

namespace Edelstein.Service.Game.Sockets
{
    public partial class WvsGameSocket
    {
        public override Task OnPacket(IPacket packet)
        {
            var operation = (RecvPacketOperations) packet.Decode<short>();

            switch (operation)
            {
                case RecvPacketOperations.MigrateIn:
                    return OnMigrateIn(packet);
                default:
                    return FieldUser?.OnPacket(operation, packet);
            }
        }

        private async Task OnMigrateIn(IPacket packet)
        {
            var characterID = packet.Decode<int>();

            using (var db = WvsGame.DataContextFactory.Create())
            {
                var character = db.Characters
                    .Include(c => c.Data)
                    .ThenInclude(a => a.Account)
                    .Include(c => c.Data)
                    .ThenInclude(c => c.Trunk)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.Inventories)
                    .ThenInclude(c => c.Items)
                    .Include(c => c.WishList)
                    .Single(c => c.ID == characterID);

                character.CoupleRecords = db.CoupleRecord
                    .Where(c => c.CharacterID == character.ID)
                    .ToList();
                character.FriendRecords = db.FriendRecord
                    .Where(c => c.CharacterID == character.ID)
                    .ToList();
                character.Memos = db.Memos
                    .Where(m => m.CharacterID == character.ID)
                    .ToList();

                if (!await WvsGame.TryMigrateFrom(character, WvsGame.Info))
                    await Disconnect();

                var field = WvsGame.FieldManager.Get(character.FieldID);
                var fieldUser = new FieldUser(this, character);

                FieldUser = fieldUser;
                await field.Enter(fieldUser);

                var memos = character.Memos;
                if (memos.Count > 0)
                {
                    using (var p = new Packet(SendPacketOperations.MemoResult))
                    {
                        p.Encode<byte>((byte) MemoResult.Load);
                        p.Encode<byte>((byte) memos.Count);
                        memos.ForEach(m =>
                        {
                            p.Encode<int>(m.ID);
                            p.Encode<string>(m.Sender);
                            p.Encode<string>(m.Content);
                            p.Encode<DateTime>(m.DateSent);
                            p.Encode<byte>((byte) m.Flag);
                        });
                        await SendPacket(p);
                    }
                }
            }
        }
    }
}