using System.Linq;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Social.Messages;
using Edelstein.Network.Packets;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services
{
    public partial class GameService
    {
        private void HandleSocialInitParty(SocialInitPartyMessage message)
        {
            var user = FieldManager
                .GetAll()
                .Select(f => f.GetObject<FieldUser>(message.CharacterID))
                .FirstOrDefault(u => u != null);
            if (user == null) return;

            using var p = new Packet(SendPacketOperations.PartyResult);
            p.Encode<byte>(0x7);
            p.Encode<int>(message.Data.ID);
            message.Data.Encode(p);
            user.SendPacket(p);
        }
    }
}