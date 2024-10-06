using Edelstein.Protocol.Gameplay.Entities.Inventories;

namespace Edelstein.Protocol.Gameplay.Constants;

public static class ItemConstants
{
    public static ItemInventoryType GetInventoryType(this int itemID)
        => (ItemInventoryType)(itemID / 1_000_000);
    
    public static bool IsStatChangeItem(this int itemID)
        => itemID / 10000 is 
            200 or 
            201 or
            202 or
            205 or 
            221 or
            236 or
            238 or
            245;
    
    public static bool IsItemOptionUpgradeItem(this int itemID) 
        => itemID / 100 == 20494;

    public static bool IsRechargeableItem(this int itemID)
        => itemID / 10000 is 207 or 233;

    public static bool IsReleaseItem(this int itemID) 
        => itemID / 10000 == 246;

    public static CashItemType GetCashItemType(this int itemID)
        => (itemID / 10000) switch
        {
            500 => CashItemType.Pet,
            501 => CashItemType.Effect,
            502 => CashItemType.Bullet,
            503 => CashItemType.ShopEmployee,
            504 => CashItemType.MapTransfer,
            505 => itemID % 10 != 0 
                ? CashItemType.SkillChange 
                : CashItemType.StatChange,
            506 => (itemID / 1000) switch
            {
                5061 => CashItemType.ExpiredProtecting,
                5062 => CashItemType.ItemUnrelease,
                _ => (itemID % 10) switch
                {
                    0 => CashItemType.Naming,
                    1 => CashItemType.Protecting,
                    2 or 
                    3 => CashItemType.Incubator,
                    _ => CashItemType.None
                }
            },
            507 => (itemID % 10000 / 1000) switch
            {
                1 => CashItemType.SpeakerChannel,
                2 => CashItemType.SpeakerWorld,
                4 => CashItemType.SkullSpeaker,
                5 => (itemID % 10) switch
                {
                    0 => CashItemType.MapleTV,
                    1 => CashItemType.MapleSoleTV,
                    2 => CashItemType.MapleLoveTV,
                    3 => CashItemType.MegaTV,
                    4 => CashItemType.MegaSoleTV,
                    5 => CashItemType.MegaLoveTV,
                    _ => CashItemType.ItemSpeaker
                },
                6 => CashItemType.ItemSpeaker,
                7 => CashItemType.ArtSpeakerWorld,
                8 => CashItemType.SpeakerBridge,
                _ => CashItemType.None
            },
            508 => CashItemType.MessageBox,
            509 => CashItemType.SendMemo,
            510 => CashItemType.Jukebox,
            512 => CashItemType.Weather,
            513 => CashItemType.ProtectOnDie,
            514 => CashItemType.Shop,
            515 => (itemID / 1000) switch
            {
                5150 or 
                5151 or
                5154 => CashItemType.Hair,
                5152 => (itemID / 100) switch
                {
                    51520 => CashItemType.Face,
                    51521 => CashItemType.ColorLens,
                    _ => CashItemType.None
                },
                5153 => CashItemType.Skin,
                _ => CashItemType.None
            },
            516 => CashItemType.Emotion,
            517 => itemID == 5170000
                ? CashItemType.SetPetName
                : CashItemType.None,
            518 => CashItemType.SetPetLife,
            519 => CashItemType.PetSkill,
            520 => CashItemType.MoneyPocket,
            522 => CashItemType.GachaponCoupon,
            523 => CashItemType.ShopScanner,
            524 => CashItemType.PetFood,
            525 => itemID == 5251100
                ? CashItemType.InvitationTicket
                : CashItemType.WeddingTicket,
            528 => (itemID / 1000) switch
            {
                5280 => CashItemType.ConsumeEffectItem,
                5281 => CashItemType.ConsumeAreaBuffItem,
                _ => CashItemType.None
            },
            530 => CashItemType.Morph,
            533 => CashItemType.QuickDelivery,
            537 => CashItemType.AdBoard,
            538 => CashItemType.PetEvol,
            539 => CashItemType.AvatarMegaphone,
            540 => (itemID / 1000) switch
            {
                5400 => CashItemType.ChangeCharacterName,
                5401 => CashItemType.TransferWorldCoupon,
                _ => CashItemType.None
            },
            542 => (itemID / 1000) switch
            {
                5420 => CashItemType.HairShopMembershipCoupon,
                _ => CashItemType.None
            },
            543 => (itemID / 1000) switch
            {
                5430 => CashItemType.CharacterSale,
                _ => CashItemType.None
            },
            545 => (itemID / 1000) switch
            {
                5451 => CashItemType.GachaponRemote,
                _ => CashItemType.SelectNPC
            },
            546 => CashItemType.PetSnack,
            547 => CashItemType.RemoteShop,
            549 => CashItemType.GachaponBoxMasterKey,
            550 => CashItemType.ExtendExpireDate,
            551 => CashItemType.UpgradeTomb,
            552 => CashItemType.KarmaScissors,
            553 => CashItemType.Reward,
            557 => CashItemType.ItemUpgrade,
            561 => CashItemType.Vega,
            562 => CashItemType.MasteryBook,
            564 => CashItemType.RecoverUpgradeCount,
            566 => CashItemType.QuestDelivery,
            _ => CashItemType.None
        };
}
