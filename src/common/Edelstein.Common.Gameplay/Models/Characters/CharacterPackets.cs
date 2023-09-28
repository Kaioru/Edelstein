using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Constants;
using Edelstein.Common.Gameplay.Models.Inventories.Items;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories;
using Edelstein.Protocol.Gameplay.Models.Inventories.Items;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Gameplay.Models.Characters;

public static class CharacterPackets
{
    public static void WriteCharacterData(
        this IPacketWriter writer,
        ICharacter character,
        DbFlags flags = DbFlags.All
    )
    {
        writer.WriteLong((long)flags);
        writer.WriteByte(0);
        writer.WriteByte(0);

        if (flags.HasFlag(DbFlags.Character))
        {
            writer.WriteCharacterStats(character);

            writer.WriteByte(character.FriendMax);
            writer.WriteBool(false);
        }

        if (flags.HasFlag(DbFlags.Money)) writer.WriteInt(character.Money);

        if (flags.HasFlag(DbFlags.InventorySize))
        {
            if (flags.HasFlag(DbFlags.ItemSlotEquip))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Equip]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotConsume))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Consume]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotInstall))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Install]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotEtc))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Etc]?.SlotMax ?? 24));
            if (flags.HasFlag(DbFlags.ItemSlotCash))
                writer.WriteByte((byte)(character.Inventories[ItemInventoryType.Cash]?.SlotMax ?? 24));
        }

        if (flags.HasFlag(DbFlags.AdminShopCount))
        {
            writer.WriteInt(0);
            writer.WriteInt(0);
        }

        if (flags.HasFlag(DbFlags.ItemSlotEquip))
        {
            var inventory = character.Inventories[ItemInventoryType.Equip]?.Items ?? new Dictionary<short, IItemSlot>();
            var equip = inventory.Where(kv => kv.Key >= 0);
            var equipped = inventory.Where(kv => kv.Key is >= -100 and < 0);
            var equipped2 = inventory.Where(kv => kv.Key is >= -1000 and < -100);
            var dragon = inventory.Where(kv => kv.Key is >= -1100 and < -1000);
            var mechanic = inventory.Where(kv => kv.Key is >= -1200 and < -1100);

            foreach (var items in new[] { equipped, equipped2, equip, dragon, mechanic })
            {
                foreach (var kv in items)
                {
                    writer.WriteShort((short)Math.Abs(kv.Key % 100));
                    writer.WriteItemData(kv.Value);
                }

                writer.WriteShort(0);
            }
        }

        foreach (var t in new List<(DbFlags, ItemInventoryType)>
                     {
                         (DbFlags.ItemSlotConsume, ItemInventoryType.Consume),
                         (DbFlags.ItemSlotInstall, ItemInventoryType.Install),
                         (DbFlags.ItemSlotEtc, ItemInventoryType.Etc),
                         (DbFlags.ItemSlotCash, ItemInventoryType.Cash)
                     }
                     .Where(t => flags.HasFlag(t.Item1)))
        {
            var items = character.Inventories[t.Item2]?.Items ?? new Dictionary<short, IItemSlot>();

            foreach (var kv in items)
            {
                writer.WriteByte((byte)kv.Key);
                writer.WriteItemData(kv.Value);
            }

            writer.WriteByte(0);
        }

        if (flags.HasFlag(DbFlags.SkillRecord))
        {
            writer.WriteShort((short)character.Skills.Records.Count);
            
            foreach (var record in character.Skills.Records)
            {
                writer.WriteInt(record.Key);
                writer.WriteInt(record.Value.Level);
                writer.WriteDateTime(record.Value.DateExpire ?? DateTime.FromFileTimeUtc(150842304000000000));

                if (SkillConstants.IsSkillNeedMasterLevel(record.Key))
                    writer.WriteInt(record.Value.MasterLevel ?? 0);
            }
        }

        if (flags.HasFlag(DbFlags.SkillCooltime)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.QuestRecord))
        {
            writer.WriteShort((short)character.QuestRecords.Records.Count);
            foreach (var kv in character.QuestRecords.Records)
            {
                writer.WriteShort((short)kv.Key);
                writer.WriteString(kv.Value.Value);
            }
        }

        if (flags.HasFlag(DbFlags.QuestComplete))
        {
            writer.WriteShort((short)character.QuestCompletes.Records.Count);
            foreach (var kv in character.QuestCompletes.Records)
            {
                writer.WriteShort((short)kv.Key);
                writer.WriteDateTime(kv.Value.DateFinish);
            }
        }

        if (flags.HasFlag(DbFlags.MinigameRecord)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.CoupleRecord))
        {
            writer.WriteShort(0); // Couple
            writer.WriteShort(0); // Friend
            writer.WriteShort(0); // Marriage
        }

        if (flags.HasFlag(DbFlags.MapTransfer))
        {
            for (var i = 0; i < 5; i++) writer.WriteInt(0);
            for (var i = 0; i < 10; i++) writer.WriteInt(0);
        }

        if (flags.HasFlag(DbFlags.NewYearCard)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.QuestRecordEx))
        {
            writer.WriteShort((short)character.QuestRecordsEx.Records.Count);
            foreach (var kv in character.QuestRecordsEx.Records)
            {
                writer.WriteShort((short)kv.Key);
                writer.WriteString(kv.Value.Value);
            }
        }

        if (flags.HasFlag(DbFlags.WildHunterInfo))
            if (JobConstants.GetJobRace(character.Job) == 3 && JobConstants.GetJobType(character.Job) == 3)
            {
                writer.WriteByte(character.WildHunterInfo.RidingType);
                for (var i = 0; i < 5; i++) 
                    writer.WriteInt(character.WildHunterInfo.CaptureMob.ElementAtOrDefault(i));
            }

        if (flags.HasFlag(DbFlags.QuestCompleteOld)) writer.WriteShort(0);

        if (flags.HasFlag(DbFlags.VisitorLog)) writer.WriteShort(0);
    }

    public static void WriteCharacterStats(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteInt(character.ID);
        writer.WriteString(character.Name, 13);

        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);
        writer.WriteInt(character.Hair);

        writer.WriteLong(0);
        writer.WriteLong(0);
        writer.WriteLong(0);

        writer.WriteByte(character.Level);
        writer.WriteShort(character.Job);
        writer.WriteShort(character.STR);
        writer.WriteShort(character.DEX);
        writer.WriteShort(character.INT);
        writer.WriteShort(character.LUK);
        writer.WriteInt(character.HP);
        writer.WriteInt(character.MaxHP);
        writer.WriteInt(character.MP);
        writer.WriteInt(character.MaxMP);

        writer.WriteShort(character.AP);
        if (JobConstants.IsExtendSPJob(character.Job))
            writer.WriteCharacterExtendSP(character.ExtendSP);
        else 
            writer.WriteShort(character.SP);

        writer.WriteInt(character.EXP);
        writer.WriteShort(character.POP);

        writer.WriteInt(character.TempEXP);
        writer.WriteInt(character.FieldID);
        writer.WriteByte(character.FieldPortal);
        writer.WriteInt(character.PlayTime);
        writer.WriteShort(character.SubJob);
    }

    public static void WriteCharacterLooks(
        this IPacketWriter writer,
        ICharacter character
    )
    {
        writer.WriteByte(character.Gender);
        writer.WriteByte(character.Skin);
        writer.WriteInt(character.Face);

        writer.WriteBool(false);
        writer.WriteInt(character.Hair);

        var inventory = character.Inventories[ItemInventoryType.Equip]?.Items ?? ImmutableDictionary<short, IItemSlot>.Empty;
        var unseen = new int[60];
        var equip = new int[60];

        if ((character.Job == Job.EvanJr || JobConstants.GetJobRace(character.Job) == 2 && JobConstants.GetJobType(character.Job) == 2) &&
            character.QuestCompletes[QuestRecords.EvanGlove] != null)
            equip[(int)BodyPart.Gloves] = 1082262;
        
        foreach (var kv in inventory.Where(kv => kv.Key < -100))
        {
            var id = Math.Abs(kv.Key) - 100;
            if (id == (int)BodyPart.Weapon) continue;
            equip[id] = kv.Value.ID;
        }
        
        foreach (var kv in inventory.Where(kv => kv.Key is < 0 and > -100))
        {
            var id = Math.Abs(kv.Key);
            if (equip[id] == 0) equip[id] = kv.Value.ID;
            else unseen[id] = kv.Value.ID;
        }

        for (byte i = 0; i < equip.Length; i++)
        {
            var value = equip[i];
            if (value == 0) continue;

            writer.WriteByte(i);
            writer.WriteInt(value);
        }
        writer.WriteByte(0xFF);
        
        for (byte i = 0; i < unseen.Length; i++)
        {
            var value = unseen[i];
            if (value == 0) continue;

            writer.WriteByte(i);
            writer.WriteInt(value);
        }
        writer.WriteByte(0xFF);
        
        writer.WriteInt(inventory.TryGetValue(-((int)BodyPart.Weapon + 100), out var weaponSticker) 
            ? weaponSticker.ID
            : 0
        );

        for (var i = 0; i < 3; i++)
            writer.WriteInt(0);
    }

    public static void WriteCharacterExtendSP(this IPacketWriter p, ICharacterExtendSP extendSP)
    {
        p.WriteByte((byte)extendSP.Records.Count);
        foreach (var kv in extendSP.Records)
        {
            p.WriteByte(kv.Key);
            p.WriteByte(kv.Value);
        }
    }
}
