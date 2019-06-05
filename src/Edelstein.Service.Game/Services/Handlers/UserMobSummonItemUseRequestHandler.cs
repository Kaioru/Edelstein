using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.Item.Consume;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Fields.Objects.User;
using MoreLinq;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserMobSummonItemUseRequestHandler : AbstractItemUseHandler<MobSummonItemTemplate>
    {
        public override async Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            MobSummonItemTemplate template,
            ItemSlot item)
        {
            var random = new Random();

            await Task.WhenAll(template.Mobs
                .Where(m => random.Next(100) <= m.Prob)
                .Select(m =>
                {
                    var mob = user.Service.TemplateManager.Get<MobTemplate>(m.TemplateID);
                    return user.Field.Enter(new FieldMob(mob, random.NextDouble() >= 0.5)
                    {
                        Position = user.Position,
                        Foothold = user.Foothold
                    });
                }));
        }
    }
}