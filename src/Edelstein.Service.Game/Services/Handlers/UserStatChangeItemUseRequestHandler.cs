using System;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item.Consume;
using Edelstein.Service.Game.Extensions;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Stats;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserStatChangeItemUseRequestHandler : AbstractItemUseHandler<StatChangeItemTemplate>
    {
        public override async Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            StatChangeItemTemplate template,
            ItemSlot item
        )
        {
            var stats = template.GetTemporaryStats();

            if (stats.Count > 0)
            {
                await user.ModifyTemporaryStats(ts =>
                {
                    if (template.Time > 0)
                    {
                        var expire = DateTime.Now.AddSeconds(template.Time);
                        stats.ForEach(t => ts.Set(t.Key, -template.ID, t.Value, expire));
                    }
                    else stats.ForEach(t => ts.Set(t.Key, -template.ID, t.Value));
                });
            }

            if (!stats.ContainsKey(TemporaryStatType.Morph))
            {
                var incHP = 0;
                var incMP = 0;

                incHP += template.HP;
                incMP += template.MP;
                incHP += user.BasicStat.MaxHP * (template.HPr / 100);
                incMP += user.BasicStat.MaxMP * (template.MPr / 100);

                if (incHP > 0 || incMP > 0)
                {
                    await user.ModifyStats(s =>
                    {
                        s.HP += incHP;
                        s.MP += incMP;
                    });
                }
            }

            await user.ModifyInventory(i => i.Remove(item, 1), true);
        }
    }
}