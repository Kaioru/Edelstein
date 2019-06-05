using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Extensions;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Effects.User;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserSkillUseRequestHandler : AbstractFieldUserHandler
    {
        public override async Task Handle(RecvPacketOperations operation, IPacket packet, FieldUser user)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();
            var template = user.Service.TemplateManager.Get<SkillTemplate>(templateID);
            var skillLevel = user.Character.GetSkillLevel(templateID);

            if (template == null) return;
            if (skillLevel <= 0) return;

            var level = template.LevelData[skillLevel];
            var stats = level.GetTemporaryStats();

            if (stats.Count > 0)
            {
                await user.ModifyTemporaryStats(ts =>
                {
                    if (level.Time > 0)
                    {
                        var expire = DateTime.Now.AddSeconds(level.Time);
                        stats.ForEach(t => ts.Set(t.Key, templateID, t.Value, expire));
                    }
                    else stats.ForEach(t => ts.Set(t.Key, templateID, t.Value));
                });
            }
            // TODO: party/map buffs

            await user.Effect(new SkillUseEffect(templateID, (byte) skillLevel), local: false, remote: true);
            await user.ModifyStats(exclRequest: true);
        }
    }
}