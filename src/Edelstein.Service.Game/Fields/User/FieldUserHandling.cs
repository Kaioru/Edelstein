using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Core.Gameplay.Constants;
using Edelstein.Core.Types;
using Edelstein.Database.Entities.Inventories;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Skill;
using Edelstein.Service.Game.Commands;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Fields.Objects.Drops;
using Edelstein.Service.Game.Fields.Objects.NPCs;
using Edelstein.Service.Game.Fields.User.Attacking;
using Edelstein.Service.Game.Fields.User.Effects.Types;
using Edelstein.Service.Game.Fields.User.Stats;
using Edelstein.Service.Game.Logging;
using MoreLinq.Extensions;

namespace Edelstein.Service.Game.Fields.User
{
    public partial class FieldUser
    {
        private async Task OnUserTransferFieldRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();

            var name = packet.Decode<string>();
            var portal = Field.GetPortal(name);

            await portal.Enter(this);
        }

        private async Task OnUserTransferChannelRequest(IPacket packet)
        {
            var channel = packet.Decode<byte>();

            try
            {
                var service = Socket.Service.Peers
                    .OfType<GameServiceInfo>()
                    .Where(g => g.WorldID == Socket.Service.Info.WorldID)
                    .OrderBy(g => g.ID)
                    .ToList()[channel];

                await Socket.TryMigrateTo(Socket.Account, Socket.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x1);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnUserMigrateToCashShopRequest(IPacket packet)
        {
            try
            {
                var service = Socket.Service.Peers
                    .OfType<ShopServiceInfo>()
                    .Where(g => g.Worlds.Contains(Socket.Service.Info.WorldID))
                    .OrderBy(g => g.ID)
                    .First();

                // TODO: multi selection
                await Socket.TryMigrateTo(Socket.Account, Socket.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x2);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnUserMigrateToITCRequest(IPacket packet)
        {
            try
            {
                var service = Socket.Service.Peers
                    .OfType<TradeServiceInfo>()
                    .Where(g => g.Worlds.Contains(Socket.Service.Info.WorldID))
                    .OrderBy(g => g.ID)
                    .First();

                // TODO: multi selection
                await Socket.TryMigrateTo(Socket.Account, Socket.Character, service);
            }
            catch
            {
                using (var p = new Packet(SendPacketOperations.TransferChannelReqIgnored))
                {
                    p.Encode<byte>(0x3);
                    await SendPacket(p);
                }
            }
        }

        private async Task OnUserMove(IPacket packet)
        {
            packet.Decode<long>();
            packet.Decode<byte>();
            packet.Decode<long>();
            packet.Decode<int>();
            packet.Decode<int>();
            packet.Decode<int>();

            var path = Move(packet);

            using (var p = new Packet(SendPacketOperations.UserMove))
            {
                p.Encode<int>(ID);
                path.Encode(p);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserAttack(RecvPacketOperations operation, IPacket packet)
        {
            var type = (AttackType) (operation - RecvPacketOperations.UserMeleeAttack);
            var info = new AttackInfo(type, this, packet);

            // keydown packets

            using (var p = new Packet(SendPacketOperations.UserMeleeAttack + (int) type))
            {
                p.Encode<int>(ID);
                p.Encode<byte>((byte) (info.DamagePerMob | 16 * info.MobCount));
                p.Encode<byte>(Character.Level);

                if (info.SkillID > 0)
                {
                    p.Encode<byte>((byte) Character.GetSkillLevel(info.SkillID));
                    p.Encode<int>(info.SkillID);
                }
                else p.Encode<byte>(0);

                p.Encode<byte>(0x20); // bSerialAttack
                p.Encode<short>((short) (info.Action & 0x7FFF | (Convert.ToInt16(info.Left) << 15)));

                if (info.Action <= 0x110)
                {
                    p.Encode<byte>(0); // nMastery
                    p.Encode<byte>(0); // v82
                    p.Encode<int>(2070000); // bMovingShoot

                    info.DamageInfo.ForEach(i =>
                    {
                        p.Encode<int>(i.MobID);

                        if (i.MobID <= 0) return;

                        p.Encode<byte>(i.HitAction);

                        // check 4211006

                        i.Damage.ForEach(d =>
                        {
                            p.Encode<bool>(false);
                            p.Encode<int>(d);
                        });
                    });
                }

                await Field.BroadcastPacket(this, p);
            }

            await info.Apply();
        }

        private async Task OnUserChat(IPacket packet)
        {
            packet.Decode<int>();

            var message = packet.Decode<string>();
            var onlyBalloon = packet.Decode<bool>();

            if (message.StartsWith(CommandManager.Prefix))
            {
                try
                {
                    await Service.CommandManager.Process(
                        this,
                        message.Substring(1)
                    );
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Caught exception when executing command");
                    await Message("An error has occured while executing that command.");
                }

                return;
            }

            using (var p = new Packet(SendPacketOperations.UserChat))
            {
                p.Encode<int>(ID);
                p.Encode<bool>(false);
                p.Encode<string>(message);
                p.Encode<bool>(onlyBalloon);
                await Field.BroadcastPacket(p);
            }
        }

        private async Task OnUserEmotion(IPacket packet)
        {
            var emotion = packet.Decode<int>();
            var duration = packet.Decode<int>();
            var byItemOption = packet.Decode<bool>();

            // TODO: item option checks

            using (var p = new Packet(SendPacketOperations.UserEmotion))
            {
                p.Encode<int>(ID);
                p.Encode<int>(emotion);
                p.Encode<int>(duration);
                p.Encode<bool>(byItemOption);
                await Field.BroadcastPacket(this, p);
            }
        }

        private async Task OnUserSelectNPC(IPacket packet)
        {
            var npc = Field.GetObject<FieldNPC>(packet.Decode<int>());

            if (npc == null) return;

            var template = npc.Template;
            var script = template.Scripts.FirstOrDefault()?.Script;

            if (script == null) return;

            var context = new ConversationContext(Socket);
            var conversation = await Service.ConversationManager.Build(
                script,
                context,
                new FieldNPCSpeaker(context, npc),
                new FieldUserSpeaker(context, this)
            );

            await Converse(conversation);
        }

        private async Task OnUserScriptMessageAnswer(IPacket packet)
        {
            if (ConversationContext == null) return;

            var type = (ConversationMessageType) packet.Decode<byte>();

            if (type != ConversationContext.LastRequestType) return;
            if (type == ConversationMessageType.AskQuiz ||
                type == ConversationMessageType.AskSpeedQuiz)
            {
                await ConversationContext.Respond(packet.Decode<string>());
                return;
            }

            var answer = packet.Decode<byte>();

            if (
                type != ConversationMessageType.Say &&
                type != ConversationMessageType.AskYesNo &&
                type != ConversationMessageType.AskAccept &&
                answer == byte.MinValue ||
                (type == ConversationMessageType.Say ||
                 type == ConversationMessageType.AskYesNo ||
                 type == ConversationMessageType.AskAccept) && answer == byte.MaxValue
            )
            {
                ConversationContext.TokenSource.Cancel();
                return;
            }

            switch (type)
            {
                case ConversationMessageType.AskText:
                case ConversationMessageType.AskBoxText:
                    await ConversationContext.Respond(packet.Decode<string>());
                    break;
                case ConversationMessageType.AskNumber:
                case ConversationMessageType.AskMenu:
                case ConversationMessageType.AskSlideMenu:
                    await ConversationContext.Respond(packet.Decode<int>());
                    break;
                case ConversationMessageType.AskAvatar:
                case ConversationMessageType.AskMemberShopAvatar:
                    await ConversationContext.Respond(packet.Decode<byte>());
                    break;
                case ConversationMessageType.AskYesNo:
                case ConversationMessageType.AskAccept:
                    await ConversationContext.Respond(Convert.ToBoolean(answer));
                    break;
                default:
                    await ConversationContext.Respond(answer);
                    break;
            }
        }

        private async Task OnUserChangeSlotPositionRequest(IPacket packet)
        {
            packet.Decode<int>();
            var type = (ItemInventoryType) packet.Decode<byte>();
            var from = packet.Decode<short>();
            var to = packet.Decode<short>();
            var number = packet.Decode<short>();

            if (to == 0)
            {
                await ModifyInventory(i =>
                {
                    var item = Character.Inventories[type].Items[from];

                    if (!ItemConstants.IsTreatSingly(item.TemplateID))
                    {
                        if (!(item is ItemSlotBundle bundle)) return;
                        if (bundle.Number < number) return;

                        item = i[type].Take(from, number);
                    }
                    else i[type].Remove(from);

                    var drop = new ItemFieldDrop(item) {Position = Position};
                    Field.Enter(drop, () => drop.GetEnterFieldPacket(0x1, this));
                }, true);
                return;
            }

            // TODO: equippable checks
            await ModifyInventory(i => i[type].Move(from, to), true);
        }

        private Task OnDropPickUpRequest(IPacket packet)
        {
            packet.Decode<byte>();
            packet.Decode<int>();
            packet.Decode<short>();
            packet.Decode<short>();
            var objectID = packet.Decode<int>();
            packet.Decode<int>();
            var drop = Field.GetObject<AbstractFieldDrop>(objectID);

            return drop?.PickUp(this);
        }

        private async Task OnUserSkillUpRequest(IPacket packet)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();
            var template = Service.TemplateManager.Get<SkillTemplate>(templateID);
            var job = template.ID / 10000;
            var jobLevel = (byte) SkillConstants.GetJobLevel(job);

            if (template == null) return;
            if (SkillConstants.IsExtendSPJob(job) && Character.GetExtendSP(jobLevel) <= 0) return;
            if (!SkillConstants.IsExtendSPJob(job) && Character.SP <= 0) return;

            var maxLevel = template.MaxLevel;
            if (SkillConstants.IsSkillNeedMasterLevel(templateID))
                maxLevel = (short) Character.GetSkillMasterLevel(templateID);

            if (Character.GetSkillLevel(templateID) >= maxLevel) return;

            await ModifyStats(s =>
            {
                if (SkillConstants.IsExtendSPJob(job))
                    s.SetExtendSP(jobLevel, (byte) (s.GetExtendSP(jobLevel) - 1));
                else s.SP--;
            });
            await ModifySkills(s => s.Add(templateID), true);
        }

        private async Task OnUserSkillUseRequest(IPacket packet)
        {
            packet.Decode<int>();
            var templateID = packet.Decode<int>();
            var template = Service.TemplateManager.Get<SkillTemplate>(templateID);
            var skillLevel = Character.GetSkillLevel(templateID);

            if (template == null) return;
            if (skillLevel <= 0) return;

            var stats = new Dictionary<TemporaryStatType, short>();
            var level = template.LevelData[skillLevel];

            if (level.PAD != 0) stats.Add(TemporaryStatType.PAD, level.PAD);
            if (level.PDD != 0) stats.Add(TemporaryStatType.PDD, level.PDD);
            if (level.MAD != 0) stats.Add(TemporaryStatType.MAD, level.MAD);
            if (level.MDD != 0) stats.Add(TemporaryStatType.MDD, level.MDD);
            if (level.ACC != 0) stats.Add(TemporaryStatType.ACC, level.ACC);
            if (level.EVA != 0) stats.Add(TemporaryStatType.EVA, level.EVA);
            if (level.Craft != 0) stats.Add(TemporaryStatType.Craft, level.Craft);
            if (level.Speed != 0) stats.Add(TemporaryStatType.Speed, level.Speed);
            if (level.Jump != 0) stats.Add(TemporaryStatType.Jump, level.Jump);

            if (level.Morph > 0) stats.Add(TemporaryStatType.Morph, level.Morph);

            if (level.EMHP != 0) stats.Add(TemporaryStatType.EMHP, level.EMHP);
            if (level.EMMP != 0) stats.Add(TemporaryStatType.EMMP, level.EMMP);
            if (level.EPAD != 0) stats.Add(TemporaryStatType.EPAD, level.EPAD);
            if (level.EPDD != 0) stats.Add(TemporaryStatType.EPDD, level.EPDD);
            // if (level.EMAD != 0) temporaryStats.Add(TemporaryStatType.EMAD, level.EMAD);
            if (level.EMDD != 0) stats.Add(TemporaryStatType.EMDD, level.EMDD);

            switch ((Skill) templateID)
            {
                case Skill.MagicianMagicGuard:
                case Skill.FlamewizardMagicGuard:
                case Skill.EvanMagicGuard:
                    stats.Add(TemporaryStatType.MagicGuard, level.X);
                    break;
                case Skill.RogueDarkSight:
                case Skill.NightwalkerDarkSight:
                    stats.Add(TemporaryStatType.DarkSight, level.X);
                    break;
                case Skill.FighterWeaponBooster:
                case Skill.PageWeaponBooster:
                case Skill.SpearmanWeaponBooster:
                case Skill.Mage1MagicBooster:
                case Skill.Mage2MagicBooster:
                case Skill.HunterBowBooster:
                case Skill.CrossbowmanCrossbowBooster:
                case Skill.AssassinJavelinBooster:
                case Skill.ThiefDaggerBooster:
                case Skill.Dual1DualBooster:
                case Skill.InfighterKnuckleBooster:
                case Skill.GunslingerGunBooster:
                case Skill.StrikerKnuckleBooster:
                case Skill.SoulmasterSwordBooster:
                case Skill.FlamewizardMagicBooster:
                case Skill.WindbreakerBowBooster:
                case Skill.NightwalkerJavelinBooster:
                case Skill.AranPolearmBooster:
                case Skill.EvanMagicBooster:
                case Skill.BmageStaffBooster:
                case Skill.WildhunterCrossbowBooster:
                case Skill.MechanicBooster:
                    stats.Add(TemporaryStatType.Booster, level.X);
                    break;
                case Skill.FighterPowerGuard:
                case Skill.PagePowerGuard:
                    stats.Add(TemporaryStatType.PowerGuard, level.X);
                    break;
                case Skill.NoviceHyperBody:
                case Skill.SpearmanHyperBody:
                case Skill.AdminHyperBody:
                case Skill.NoblesseHyperBody:
                case Skill.LegendHyperBody:
                case Skill.EvanjrHyperBody:
                case Skill.CitizenHyperBody:
                    stats.Add(TemporaryStatType.MaxHP, level.X);
                    stats.Add(TemporaryStatType.MaxMP, level.Y);
                    break;
                case Skill.ClericInvincible:
                    stats.Add(TemporaryStatType.Invincible, level.X);
                    break;
                case Skill.HunterSoulArrowBow:
                case Skill.CrossbowmanSoulArrowCrossbow:
                case Skill.WindbreakerSoulArrowBow:
                case Skill.WildhunterSoulArrowCrossbow:
                    stats.Add(TemporaryStatType.SoulArrow, level.X);
                    break;
                // TODO: Combo attack
                // TODO: Weapon charge
                case Skill.DragonknightDragonBlood:
                    stats.Add(TemporaryStatType.DragonBlood, level.X);
                    break;
                case Skill.PriestHolySymbol:
                case Skill.AdminHolySymbol:
                    stats.Add(TemporaryStatType.HolySymbol, level.X);
                    break;
                case Skill.HermitMesoUp:
                    stats.Add(TemporaryStatType.MesoUp, level.X);
                    break;
                case Skill.HermitShadowPartner:
                case Skill.ThiefmasterShadowPartner:
                case Skill.Dual4MirrorImaging:
                case Skill.NightwalkerShadowPartner:
                    stats.Add(TemporaryStatType.ShadowPartner, level.X);
                    break;
                case Skill.ThiefmasterPickpocket:
                    stats.Add(TemporaryStatType.PickPocket, level.X);
                    break;
                case Skill.ThiefmasterMesoGuard:
                    stats.Add(TemporaryStatType.MesoGuard, level.X);
                    break;
                case Skill.HeroMapleHero:
                case Skill.PaladinMapleHero:
                case Skill.DarkknightMapleHero:
                case Skill.Archmage1MapleHero:
                case Skill.Archmage2MapleHero:
                case Skill.BishopMapleHero:
                case Skill.BowmasterMapleHero:
                case Skill.CrossbowmasterMapleHero:
                case Skill.NightlordMapleHero:
                case Skill.ShadowerMapleHero:
                case Skill.Dual5MapleHero:
                case Skill.ViperMapleHero:
                case Skill.CaptainMapleHero:
                case Skill.AranMapleHero:
                case Skill.EvanMapleHero:
                case Skill.BmageMapleHero:
                case Skill.WildhunterMapleHero:
                case Skill.MechanicMapleHero:
                    stats.Add(TemporaryStatType.BasicStatUp, level.X);
                    break;
                case Skill.HeroStance:
                case Skill.PaladinStance:
                case Skill.DarkknightStance:
                case Skill.AranFreezeStanding:
                case Skill.BmageStance:
                    stats.Add(TemporaryStatType.Stance, level.X);
                    break;
                // TODO: sharp eyes
                case Skill.Archmage1ManaReflection:
                case Skill.Archmage2ManaReflection:
                case Skill.BishopManaReflection:
                    stats.Add(TemporaryStatType.ManaReflection, level.X);
                    break;
                case Skill.NightlordSpiritJavelin:
                    stats.Add(TemporaryStatType.SpiritJavelin, level.X);
                    break;
                case Skill.Archmage1Infinity:
                case Skill.Archmage2Infinity:
                case Skill.BishopInfinity:
                    stats.Add(TemporaryStatType.Infinity, level.X);
                    break;
                case Skill.BishopHolyShield:
                    stats.Add(TemporaryStatType.Holyshield, level.X);
                    break;
                case Skill.BowmasterHamstring:
                    stats.Add(TemporaryStatType.HamString, level.X);
                    break;
                case Skill.CrossbowmasterBlind:
                case Skill.WildhunterBlind:
                    stats.Add(TemporaryStatType.Blind, level.X);
                    break;
                case Skill.BowmasterConcentration:
                    stats.Add(TemporaryStatType.Concentration, level.X);
                    break;
                case Skill.NoviceMaxlevelEchobuff:
                case Skill.NoblesseMaxlevelEchobuff:
                case Skill.LegendMaxlevelEchobuff:
                case Skill.EvanjrMaxlevelEchobuff:
                case Skill.CitizenMaxlevelEchobuff:
                    stats.Add(TemporaryStatType.MaxLevelBuff, level.X);
                    break;
            }

            if (stats.Count > 0)
            {
                await ModifyTemporaryStats(ts =>
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

            await Effect(new SkillUseEffect(templateID, (byte) skillLevel), remote: true);
            await ModifyStats(exclRequest: true);
        }


        private async Task OnUserSkillCancelRequest(IPacket packet)
        {
            var templateID = packet.Decode<int>();
            var template = Service.TemplateManager.Get<SkillTemplate>(templateID);
            
            if (template == null) return;

            await ModifyTemporaryStats(ts => ts.Reset(templateID));
        }

        private async Task OnUserCharacterInfoRequest(IPacket packet)
        {
            packet.Decode<int>();
            var user = Field.GetObject<FieldUser>(packet.Decode<int>());

            if (user == null) return;

            using (var p = new Packet(SendPacketOperations.CharacterInfo))
            {
                var c = user.Character;

                p.Encode<int>(user.ID);
                p.Encode<byte>(c.Level);
                p.Encode<short>(c.Job);
                p.Encode<short>(c.POP); // TODO: use basic stat POP

                p.Encode<byte>(0);

                p.Encode<string>(""); // sCommunity
                p.Encode<string>(""); // sAlliance

                p.Encode<byte>(0);
                p.Encode<byte>(0);
                p.Encode<byte>(0); // TamingMobInfo
                p.Encode<byte>(0); // Wishlist

                p.Encode<int>(0); // MedalAchievementInfo
                p.Encode<short>(0);

                var chairs = c.Inventories.Values
                    .SelectMany(i => i.Items)
                    .Select(kv => kv.Value)
                    .Select(i => i.TemplateID)
                    .Where(i => i / 10000 == 301)
                    .ToList();
                p.Encode<int>(chairs.Count);
                chairs.ForEach(i => p.Encode<int>(i));
                await SendPacket(p);
            }
        }

        private async Task OnUserPortalScriptRequest(IPacket packet)
        {
            packet.Decode<byte>();

            var name = packet.Decode<string>();
            var portal = Field.Template.Portals.Values.FirstOrDefault(p => p.Name.Equals(name));

            if (portal == null) return;
            if (string.IsNullOrEmpty(portal.Script)) return;

            var context = new ConversationContext(Socket);
            var conversation = await Socket.Service.ConversationManager.Build(
                portal.Script,
                context,
                new FieldSpeaker(context, Field),
                new FieldUserSpeaker(context, this)
            );

            await Converse(conversation);
        }
    }
}