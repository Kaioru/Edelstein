using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Extensions.Templates;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.Item;
using Edelstein.Provider.Templates.Item.Reward;
using Edelstein.Service.Game.Fields.Generators;
using Edelstein.Service.Game.Fields.Movements;
using Edelstein.Service.Game.Fields.Objects.Drop;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.Objects.Mob
{
    public class FieldMob : AbstractFieldControlledLife, IFieldGeneratedObj
    {
        public override FieldObjType Type => FieldObjType.Mob;
        public MobTemplate Template { get; }
        public short HomeFoothold { get; set; }
        public IFieldGenerator Generator { get; set; }

        public int HP { get; set; }
        public int MP { get; set; }
        public int EXP { get; set; }

        public FieldMob(MobTemplate template, bool left = true)
        {
            Template = template;
            MoveAction = (byte) (
                Convert.ToByte(left) & 1 |
                2 * (byte) (Template.MoveAbility switch {
                    MoveAbilityType.Fly => MoveActionType.Fly,
                    MoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move,
                    }
                )
            );

            HP = template.MaxHP;
            MP = template.MaxHP;
            EXP = template.EXP;
        }

        public async Task Damage(FieldUser user, int damage)
        {
            lock (this) HP -= damage;

            var indicator = HP / (float) Template.MaxHP * 100f;

            indicator = Math.Min(100, indicator);
            indicator = Math.Max(0, indicator);

            using (var p = new Packet(SendPacketOperations.MobHPIndicator))
            {
                p.Encode<int>(ID);
                p.Encode<byte>((byte) indicator);
                await user.SendPacket(p);
            }

            if (HP > 0) return;

            await Field.Leave(this);
            await user.ModifyStats(s => { s.EXP += Template.EXP; });
            await user.Message(new IncEXPMessage
            {
                IsLastHit = true,
                EXP = Template.EXP
            });

            var reward = user.Service.TemplateManager.Get<RewardTemplate>(Template.ID);
            var rewards = reward?.Entries
                              .Where(e => new Random().NextDouble() <= e.Prob)
                              .Shuffle()
                              .ToList()
                          ?? new List<RewardEntryTemplate>();
            var drops = rewards
                .Select<RewardEntryTemplate, AbstractFieldDrop>(r =>
                {
                    if (r.Money > 0)
                    {
                        var money = r.Money;
                        var modifier = 2 * money / 5 + 1;

                        return new MoneyFieldDrop(Math.Max(1, 4 * money / 5 + new Random().Next() % modifier));
                    }

                    var template = user.Service.TemplateManager.Get<ItemTemplate>(r.TemplateID);
                    var item = template.ToItemSlot(ItemVariationType.Normal);

                    if (item is ItemSlotBundle bundle)
                        bundle.Number = (short) new Random().Next(r.MinQuantity, r.MaxQuantity);
                    return new ItemFieldDrop(item);
                })
                .ToList();
            var bounds = Field.Template.Bounds;
            var expire = DateTime.Now.AddMinutes(3);

            drops.ForEach(d =>
            {
                var x = Position.X + (drops.IndexOf(d) - (drops.Count - 1) / 2) * -20;
                var y = Position.Y;

                x = Math.Min(bounds.Right - 10, x);
                x = Math.Max(bounds.Left + 10, x);
                d.Position = new Point(x, y);
                d.DateExpire = expire;
            });
            await Task.WhenAll(drops
                .Select(d => Field.Enter(d, () => d.GetEnterFieldPacket(0x1, this)))
            );
        }

        private void EncodeData(IPacket packet, MobAppearType summonType, int? summonOption = null)
        {
            packet.Encode<byte>(1); // CalcDamageStatIndex
            packet.Encode<int>(Template.ID);

            packet.Encode<long>(0); // Temporary Stat
            packet.Encode<long>(0); // Temporary Stat

            packet.Encode<Point>(Position);
            packet.Encode<byte>(MoveAction);
            packet.Encode<short>(Foothold);
            packet.Encode<short>(HomeFoothold);

            packet.Encode<sbyte>((sbyte) summonType);
            if (summonType == MobAppearType.Revived ||
                summonType >= 0)
                packet.Encode<int>(summonOption ?? 0);

            packet.Encode<byte>(0);
            packet.Encode<int>(0);
            packet.Encode<int>(0);
        }
        public IPacket GetEnterFieldPacket(MobAppearType summonType, int? summonOption = null)
        {
            using (var p = new Packet(SendPacketOperations.MobEnterField))
            {
                p.Encode<int>(ID);
                EncodeData(p, summonType, summonOption);
                return p;
            }
        }


        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(MobAppearType.Normal);

        public override IPacket GetLeaveFieldPacket()
        {
            using (var p = new Packet(SendPacketOperations.MobLeaveField))
            {
                p.Encode<int>(ID);
                p.Encode<byte>(1); // m_tLastUpdateAmbush?
                return p;
            }
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            using (var p = new Packet(SendPacketOperations.MobChangeController))
            {
                p.Encode<bool>(setAsController);
                p.Encode<int>(ID);

                if (setAsController)
                    EncodeData(p, MobAppearType.Regen);
                return p;
            }
        }
    }
}