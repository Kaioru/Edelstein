using System;
using System.IO;
using Edelstein.Protocol.Gameplay.Entities.Stats.TwoState;

namespace Edelstein.Protocol.Gameplay.Entities.Stats;

internal static class StructuredTemporaryStatsExtensions
{
    internal static void WriteTSFlag(this BinaryWriter writer, ITemporaryStats stats)
    {
        var flag = stats.GetFlags();
        
        foreach (var i in flag.ToArray())
            writer.Write(i);
    }

    internal static void WriteTSTwoState(this BinaryWriter writer, ITemporaryStats stats, DateTime now)
    {
        if (stats.EnergyCharged != null) writer.WriteTSTwoStateDynamicTerm(stats.EnergyCharged, now);
        if (stats.DashSpeed != null) writer.WriteTSTwoStateDynamicTerm(stats.DashSpeed, now);
        if (stats.DashJump != null) writer.WriteTSTwoStateDynamicTerm(stats.DashJump, now);
        if (stats.RideVehicle != null) writer.WriteTSTwoStateOption(stats.RideVehicle, now);
        
        if (stats.PartyBooster != null)
        {
            writer.WriteTSTwoStateOption(stats.PartyBooster, now);
            writer.WriteTSTwoStateTerm(stats.PartyBooster.DateStart, now);
            writer.Write((short)stats.PartyBooster.Term.TotalSeconds);
        }

        if (stats.GuidedBullet != null)
        {
            writer.WriteTSTwoStateOption(stats.GuidedBullet, now);
            writer.Write(stats.GuidedBullet.MobID);
        }
        
        if (stats.Undead != null) writer.WriteTSTwoStateDynamicTerm(stats.Undead, now);
    }

    private static void WriteTSTwoStateOption(this BinaryWriter writer, TwoStateOption option, DateTime now)
    {
        writer.Write(option.Option);
        writer.Write(option.Reason);
        writer.WriteTSTwoStateTerm(option.DateUpdated, now);
    }

    private static void WriteTSTwoStateDynamicTerm(this BinaryWriter writer, TwoStateDynamicTermOption option, DateTime now)
    {
        writer.WriteTSTwoStateOption(option, now);
        writer.Write((short)option.Term.TotalSeconds);
    }

    private static void WriteTSTwoStateTerm(this BinaryWriter writer, DateTime time, DateTime now)
    {
        writer.Write(time < now);
        writer.Write((int)(time - now).TotalSeconds);
    }
}
