using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserSkillCancelRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<SkillTemplate>(templateID);

            if (template == null) return;
            if (SkillConstants.IsKeydownSkill(templateID))
            {
                using var p = new Packet(SendPacketOperations.UserSkillCancel);
                p.Encode<int>(user.ID);
                p.Encode<int>(templateID);

                await user.Field.BroadcastPacket(user, p);
            }
            else await user.ModifyTemporaryStats(ts => ts.Reset(templateID));
        }
    }
}