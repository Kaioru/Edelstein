using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob.Stats.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Stats.Modify;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.Mob
{
    public class FieldObjMob : AbstractFieldControlledLife, IFieldObjMob
    {
        public override FieldObjType Type => FieldObjType.Mob;

        public IFieldObjMobInfo Info { get; }

        public int HP { get; private set; }
        public int MP { get; private set; }

        public ICalculatedMobStats Stats { get; }

        public IMobStats MobStats { get; }

        public FieldObjMob(IFieldObjMobInfo info, bool left = true)
        {
            Info = info;
            Action = (MoveActionType)(byte)(
                Convert.ToByte(left) & 1 |
                2 * (byte)(Info.MoveAbility switch
                {
                    MoveAbilityType.Fly => MoveActionType.Fly1,
                    MoveAbilityType.Stop => MoveActionType.Stand,
                    _ => MoveActionType.Move,
                }
                )
            );

            Stats = new CalculatedMobStats(this);
            MobStats = new MobStats();

            UpdateStats().Wait();

            HP = Stats.MaxHP;
            MP = Stats.MaxHP;
        }

        public async Task UpdateStats()
        {
            await Stats.Calculate();
        }

        public void WriteData(IPacketWriter writer, FieldObjMobAppearType appearType, int? appearOption = null)
        {
            writer.WriteByte(1); // CalcDamageStatIndex
            writer.WriteInt(Info.ID);

            writer.WriteMobStats(MobStats);

            writer.WritePoint2D(Position);
            writer.WriteByte((byte)Action);
            writer.WriteShort((short)(Foothold?.ID ?? 0));
            writer.WriteShort(0); // Foothold again? TODO

            writer.WriteByte((byte)appearType);
            if (appearType == FieldObjMobAppearType.Revived ||
                appearType >= 0)
                writer.WriteInt(appearOption ?? 0);

            writer.WriteByte(0);
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        public IPacket GetEnterFieldPacket(FieldObjMobAppearType appearType, int? appearOption = null)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobEnterField);

            packet.WriteInt(ID);
            WriteData(packet, appearType, appearOption);
            return packet;
        }

        public override IPacket GetEnterFieldPacket()
            => GetEnterFieldPacket(FieldObjMobAppearType.Normal);

        public override IPacket GetLeaveFieldPacket()
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobLeaveField);

            packet.WriteInt(ID);
            packet.WriteInt(1); // m_tLastUpdateAmbush?
            return packet;
        }

        protected override IPacket GetChangeControllerPacket(bool setAsController)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MobChangeController);

            packet.WriteBool(setAsController);
            packet.WriteInt(ID);

            if (setAsController)
                WriteData(packet, FieldObjMobAppearType.Regen);
            return packet;
        }

        public async Task Damage(IFieldObjUser user, int damage)
        {
            lock (this)
            {
                HP -= Math.Min(damage, HP);
                HP = Math.Min(Math.Max(HP, 0), Stats.MaxHP);
            }

            var indicatorPacket = new UnstructuredOutgoingPacket(PacketSendOperations.MobHPIndicator);
            var indicator = HP / (float)Stats.MaxHP * 100f;

            indicator = Math.Min(100, indicator);
            indicator = Math.Max(0, indicator);

            indicatorPacket.WriteInt(ID);
            indicatorPacket.WriteByte((byte)indicator);

            await user.Dispatch(indicatorPacket);

            if (HP > 0) {
                await UpdateStats();
                return;
            }

            await Kill(user);
        }

        public async Task Kill(IFieldObjUser user)
        {
            await Field.Leave(this);
        }

        public async Task ModifyMobStats(Action<IModifyMobStatContext> action = null)
        {
            var context = new ModifyMobStatContext(MobStats as MobStats);

            action?.Invoke(context);

            if (context.ResetHistory.ToDictionary().Any())
            {
                var resetPacket = new UnstructuredOutgoingPacket(PacketSendOperations.MobStatReset);

                resetPacket.WriteInt(ID);
                resetPacket.WriteMobStatsFlag(context.ResetHistory);
                resetPacket.WriteByte(0); // CalcDamageStatIndex

                await FieldSplit.Dispatch(resetPacket);
            }

            if (context.SetHistory.ToDictionary().Any())
            {
                var setPacket = new UnstructuredOutgoingPacket(PacketSendOperations.MobStatSet);

                setPacket.WriteInt(ID);
                setPacket.WriteMobStats(context.SetHistory);
                setPacket.WriteShort(0); // tDelay
                setPacket.WriteByte(0); // CalcDamageStatIndex

                await FieldSplit.Dispatch(setPacket);
            }
        }
    }
}
