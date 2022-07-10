using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Users.Inventories;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers
{
    public class UserCharacterInfoRequestHandler : AbstractUserPacketHandler
    {
        public override short Operation => (short)PacketRecvOperations.UserCharacterInfoRequest;

        protected override async Task Handle(GameStageUser stageUser, IFieldObjUser user, IPacketReader packet)
        {
            _ = packet.ReadInt();
            var targetID = packet.ReadInt();
            var target = user.Watching
                .SelectMany(w => w.GetObjects<IFieldObjUser>())
                .Where(o => o.ID == targetID)
                .FirstOrDefault();

            if (target == null) return;

            var response = new UnstructuredOutgoingPacket(PacketSendOperations.CharacterInfo);

            response.WriteInt(target.ID);
            response.WriteByte(target.Character.Level);
            response.WriteShort(target.Character.Job);
            response.WriteShort(target.Character.POP);

            response.WriteByte(0);

            response.WriteString(target?.Guild?.Name ?? "");
            response.WriteString(""); // sAlliance

            response.WriteByte(0); // Medal?

            response.WriteBool(false); // Pets

            response.WriteByte(0); // TamingMobInfo
            response.WriteByte(0); // Wishlist

            response.WriteInt(0); // MedalAchievementInfo
            response.WriteShort(0);

            var chairs = target.Character.Inventories[ItemInventoryType.Install].Items
                .Select(kv => kv.Value)
                .Select(i => i.TemplateID)
                .Where(i => i / 10000 == 301)
                .ToList();
            response.WriteInt(chairs.Count);
            chairs.ForEach(i => response.WriteInt(i));

            await user.Dispatch(response);
        }
    }
}
