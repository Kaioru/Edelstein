namespace Edelstein.Common.Gameplay.Shop;

public enum ShopResultOperations
{
    CancelNameChangeFail = 0x34,

    CharacterSaleSuccess = 0x38,
    CharacterSaleFail = 0x39,
    CharacterSaleInvalidName = 0x3A,
    CharacterSaleInvalidItem = 0x3B,

    ItemUpgradeSuccess = 0x3D,
    ItemUpgradeDone = 0x41,
    ItemUpgradeErr = 0x42,

    VegaSuccess1 = 0x44,
    VegaSuccess2 = 0x45,
    VegaErr = 0x46,
    VegaErr2 = 0x47,
    VegaErr_InvalidItem = 0x48,
    VegaFail = 0x49,

    CheckFreeCashItemTable_Done = 0x4F,
    CheckFreeCashItemTable_Failed = 0x50,

    SetFreeCashItemTable_Done = 0x52,
    SetFreeCashItemTable_Failed = 0x53,

    LimitGoodsCount_Changed = 0x54,

    WebShopOrderGetList_Done = 0x55,
    WebShopOrderGetList_Failed = 0x56,
    WebShopReceive_Done = 0x57,

    LoadLocker_Done = 0x58,
    LoadLocker_Failed = 0x59,

    LoadGift_Done = 0x5A,
    LoadGift_Failed = 0x5B,

    LoadWish_Done = 0x5C,
    LoadWish_Failed = 0x5D,

    MapleTV_Failed_Wrong_User_Name = 0x5E,
    MapleTV_Failed_User_Not_Connected = 0x5F,

    AvatarMegaphone_Queue_Full = 0x60,
    AvatarMegaphone_Level_Limit = 0x61,

    SetWish_Done = 0x62,
    SetWish_Failed = 0x63,

    Buy_Done = 0x64,
    Buy_Failed = 0x65,

    UseCoupon_Done = 0x66,
    UseCoupon_Done_NormalItem = 0x67,
    GiftCoupon_Done = 0x68,
    UseCoupon_Failed = 0x69,
    UseCoupon_CashItem_Failed = 0x6A,

    Gift_Done = 0x6B,
    Gift_Failed = 0x6C,

    IncSlotCount_Done = 0x6D,
    IncSlotCount_Failed = 0x6E,

    IncTrunkCount_Done = 0x6F,
    IncTrunkCount_Failed = 0x70,

    IncCharSlotCount_Done = 0x71,
    IncCharSlotCount_Failed = 0x72,

    IncBuyCharCount_Done = 0x73,
    IncBuyCharCount_Failed = 0x74,

    EnableEquipSlotExt_Done = 0x75,
    EnableEquipSlotExt_Failed = 0x76,

    MoveLtoS_Done = 0x77,
    MoveLtoS_Failed = 0x78,

    MoveStoL_Done = 0x79,
    MoveStoL_Failed = 0x7A,

    Destroy_Done = 0x7B,
    Destroy_Failed = 0x7C,

    Expire_Done = 0x7D,
    Expire_Failed = 0x7E,

    Use_Done = 0x7F,
    Use_Failed = 0x80,

    StatChange_Done = 0x81,
    StatChange_Failed = 0x82,

    SkillChange_Done = 0x83,
    SkillChange_Failed = 0x84,

    SkillReset_Done = 0x85,
    SkillReset_Failed = 0x86,

    DestroyPetItem_Done = 0x87,
    DestroyPetItem_Failed = 0x88,

    SetPetName_Done = 0x89,
    SetPetName_Failed = 0x8A,

    SetPetLife_Done = 0x8B,
    SetPetLife_Failed = 0x8C,

    MovePetStat_Failed = 0x8D,
    MovePetStat_Done = 0x8E,

    SetPetSkill_Failed = 0x8F,
    SetPetSkill_Done = 0x90,

    SendMemo_Done = 0x91,
    SendMemo_Warning = 0x92,
    SendMemo_Failed = 0x93,

    GetMaplePoint_Done = 0x94,
    GetMaplePoint_Failed = 0x95,

    Rebate_Done = 0x96,
    Rebate_Failed = 0x97,

    Couple_Done = 0x98,
    Couple_Failed = 0x99,

    BuyPackage_Done = 0x9A,
    BuyPackage_Failed = 0x9B,

    GiftPackage_Done = 0x9C,
    GiftPackage_Failed = 0x9D,

    BuyNormal_Done = 0x9E,
    BuyNormal_Failed = 0x9F,

    ApplyWishListEvent_Done = 0xA0,
    ApplyWishListEvent_Failed = 0xA1,

    Friendship_Done = 0xA2,
    Friendship_Failed = 0xA3,

    LoadExceptionList_Done = 0xA4,
    LoadExceptionList_Failed = 0xA5,

    UpdateExceptionList_Done = 0xA6,
    UpdateExceptionList_Failed = 0xA7,

    LoadFreeCashItem_Done = 0xA8,
    LoadFreeCashItem_Failed = 0xA9,

    FreeCashItem_Done = 0xAA,
    FreeCashItem_Failed = 0xAB,

    Script_Done = 0xAC,
    Script_Failed = 0xAD,

    Bridge_Failed = 0xAE,

    PurchaseRecord_Done = 0xAF,
    PurchaseRecord_Failed = 0xB0,

    EvolPet_Failed = 0xB1,
    EvolPet_Done = 0xB2,

    NameChangeBuy_Done = 0xB3,
    NameChangeBuy_Failed = 0xB4,

    TransferWorld_Done = 0xB5,
    TransferWorld_Failed = 0xB6,

    CashGachaponOpen_Done = 0xB7,
    CashGachaponOpen_Failed = 0xB8,

    CashGachaponCopy_Done = 0xB9,
    CashGachaponCopy_Failed = 0xBA,

    ChangeMaplePoint_Done = 0xBB,
    ChangeMaplePoint_Failed = 0xBC,

    Give_Done = 0xBE,
    Give_Failed = 0xBF,

    GashItemGachapon_Failed = 0xC0,
    CashItemGachapon_Done = 0xC1
}
