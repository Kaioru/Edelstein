using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserSkillPrepareRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<SkillTemplate>(templateID);
            var skillLevel = user.Character.GetSkillLevel(templateID);

            if (template == null) return;
            if (skillLevel <= 0) return;
            if (!SkillConstants.IsKeydownSkill(templateID)) return;

            using (var p = new Packet(SendPacketOperations.UserSkillPrepare))
            {
                p.Encode<int>(user.ID);
                p.Encode<int>(templateID);
                p.Encode<byte>(packet.Decode<byte>());
                p.Encode<short>(packet.Decode<short>());
                p.Encode<byte>(packet.Decode<byte>());

                await user.Field.BroadcastPacket(user, p);
            }
        }
    }
}