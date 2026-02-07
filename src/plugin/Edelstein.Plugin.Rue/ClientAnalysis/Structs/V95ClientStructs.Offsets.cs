namespace Edelstein.Plugin.Rue.ClientAnalysis;

public static partial class V95ClientStructs
{
    public static class Offsets
    {
        public static class CWvsContext
        {
            public const int FirstUserLoad = 0x0008;
            public const int AvatarMegaphone = 0x000C;
            public const int AmLastUpdate = 0x0010;
            public const int TargetPositionX = 0x0014;
            public const int TargetPositionY = 0x0018;
            public const int ChaseEnable = 0x001C;
            public const int PetHelpPopUpShown = 0x0020;
            public const int Cookie = 0x0024;
            public const int CookieValid = 0x2024;
            public const int CookieLoadedByArgString = 0x2028;
            public const int LoginBaseStep = 0x202C;
            public const int AccountId = 0x2030;
            public const int Gender = 0x2034;
            public const int GradeCode = 0x2038;
            public const int SubGradeCode = 0x2044;
            public const int EmailAccount = 0x2050;
            public const int NexonClubId = 0x2054;
            public const int CountryId = 0x2058;
            public const int PurchaseExp = 0x2059;
            public const int WorldId = 0x205C;
            public const int ChannelId = 0x2060;
            public const int Premium = 0x2064;
            public const int PremiumArgument = 0x2068;
            public const int ChatBlockReason = 0x206C;
            public const int ChatUnblockDate = 0x206E;
            public const int RegisterDate = 0x207E;
            public const int NumOfCharacter = 0x2090;
            public const int ThisAccountJustCreatedCharacter = 0x2094;
            public const int IsGuestAccount = 0x2098;
            public const int ManagerAccount = 0x209C;
            public const int CharacterCount = 0x20A0;
            public const int SlotCount = 0x20A4;
            public const int ClientKey = 0x20A8;
            public const int TesterAccount = 0x20B0;
            public const int CharacterId = 0x20B4;
            public const int ExclRequestSent = 0x20B8;
            public const int ExclRequestSentTime = 0x20BC;
            public const int ExclRequestSentQ = 0x20C0;
            public const int CharacterData = 0x20C8;
            public const int BasicStat = 0x20D0;
            public const int SecondaryStat = 0x2148;
            public const int ForcedStat = 0x3538;
            public const int TemporaryStatView = 0x35D4;
            public const int TownPortal = 0x35EC;
            public const int ActiveEffectItemId = 0x3600;
            public const int PartyId = 0x3604;
            public const int Party = 0x3608;
            public const int FriendArray = 0x3784;
            public const int MarriedPartnerCurFieldId = 0x378C;
            public const int MarriedPartnerId = 0x3790;
            public const int PartySearchSetting = 0x3794;
            public const int PartySearchState = 0x37A8;
            public const int KeepPartySearch = 0x37AC;
            public const int Guild = 0x37C8;
            public const int DirectionMode = 0x3850;
            public const int StandAloneMode = 0x3854;
            public const int ShowUi = 0x3F28;
            public const int ChannelName = 0x3F74;
            public const int AdultChannel = 0x3F78;
            public const int ScreenWidth = 0x41B8;
            public const int ScreenHeight = 0x41BC;
        }

        public static class CLogin
        {
            public const int ConnectionDlg = 0x148;
            public const int IsWaitingVac = 0x150;
            public const int IsVacDlgOn = 0x154;
            public const int SentTimeVacPacket = 0x158;
            public const int CountRelatedSvrs = 0x15C;
            public const int CountCharacters = 0x160;
            public const int CountDataReceivedCharacters = 0x164;
            public const int RecommendWorld = 0x168;
            public const int AvatarDataVac = 0x16C;
            public const int RankVac = 0x170;
            public const int CharacterId = 0x174;
            public const int CharacterName = 0x178;
            public const int WorldId = 0x17C;
            public const int LockAvatar = 0x180;
            public const int LockCountSvr = 0x188;
            public const int LockCharacter = 0x190;
            public const int LayerBook = 0x198;
            public const int FadeOutLoginStep = 0x19C;
            public const int StartFadeOut = 0x1A0;
            public const int LoginStep = 0x1A4;
            public const int StepChanging = 0x1A8;
            public const int RequestSent = 0x1AC;
            public const int LoginOpt = 0x1B0;
            public const int QuerySsnOnCreateNewCharacter = 0x1B4;
            public const int SlotCount = 0x1B8;
            public const int BuyCharCount = 0x1BC;
            public const int BaseStep = 0x1C0;
            public const int Terminate = 0x1C4;
            public const int FocusedUi = 0x1C8;
            public const int WorldItem = 0x1CC;
            public const int CharSelected = 0x1D0;
            public const int AvatarData = 0x1D4;
            public const int Rank = 0x1D8;
            public const int OnFamily = 0x1DC;
            public const int NewEquip = 0x1E0;
            public const int RegStatId = 0x1F4;
            public const int LayerLight = 0x1F8;
            public const int LayerDust = 0x1FC;
            public const int NewAvatar = 0x200;
            public const int LoginStart = 0x208;
            public const int LoginDesc0 = 0x210;
            public const int LoginDesc1 = 0x218;
            public const int ChildModal = 0x220;
            public const int EventCharacterId = 0x228;
            public const int BalloonCount = 0x22C;
            public const int Balloon = 0x230;
            public const int LatestConnectedWorldId = 0x234;
            public const int RecommendWorldMsgLoaded = 0x238;
            public const int RecommendWorldMsg = 0x23C;
            public const int CurSelectedRace = 0x240;
            public const int CurSelectedSubJob = 0x244;
            public const int Cmd = 0x248;
            public const int FadeInRemain = 0x25C;
            public const int NeedAgreement = 0x260;
            public const int Gender = 0x264;
            public const int SubStep = 0x268;
            public const int SubStepChanged = 0x26C;
            public const int CheckedName = 0x270;
            public const int MaleItem = 0x274;
            public const int FemaleItem = 0x298;
            public const int CharSale = 0x2BC;
            public const int CharSaleJob = 0x2C0;
            public const int CanHaveExtraChar = 0x2C4;
        }

        public static class CUIChannelSelect
        {
            public const int Login = 0xF4;
            public const int UserPopulation = 0xF8;
            public const int Select = 0xFC;
            public const int WorldItem = 0x100;
            public const int CanvasWorldName = 0x104;
            public const int CanvasGauge = 0x108;
            public const int LayerSelect = 0x10C;
            public const int LayerDummy = 0x110;
            public const int Check = 0x114;
            public const int LayerScroll = 0x118;
            public const int LayerEventDesc = 0x11C;
            public const int BtCs = 0x120;
            public const int ConnectionDlg = 0x128;
        }

        public static class CUIWorldSelect
        {
            public const int Login = 0x94;
            public const int World = 0x98;
            public const int KeyFocus = 0x9C;
            public const int BtWs = 0xA0;
            public const int LayerWorldInfo = 0x1C0;
            public const int LayerBalloon = 0x1C4;
            public const int LayerWorldState = 0x1C8;
            public const int BalloonCount = 0x1CC;
            public const int LayerAdvice = 0x1D0;
            public const int WorldIdx = 0x1D4;
        }

        public static class CQuestMan
        {
            public const int SeriesQuest = 0x04;
            public const int SeriesQuestName = 0x1C;
            public const int QuestName = 0x34;
            public const int BlockedQuest = 0x4C;
            public const int WorldId = 0x64;
            public const int StartDemand = 0x68;
            public const int CompleteDemand = 0x80;
            public const int DisallowedDelivery = 0x98;
            public const int QuestCategoryName = 0x9C;
            public const int Quest = 0xA0;
            public const int NpcQuest = 0xA4;
            public const int ItemQuest = 0xBC;
            public const int ItemQuestDemand = 0xD4;
            public const int MesoQuest = 0xEC;
            public const int LevelQuest = 0xF0;
            public const int PartyQuestIconPath = 0xF4;
            public const int QuestSortKey = 0x10C;
            public const int ShowLayerTag = 0x124;
            public const int ShowEffect = 0x13C;
            public const int ModifiedQuestTime = 0x154;
            public const int Exclusive = 0x158;
            public const int AutoStartQuest = 0x170;
            public const int AutoAcceptQuest = 0x188;
            public const int AutoCompleteQuest = 0x1A0;
            public const int AutoCancelQuest = 0x1B8;
            public const int OneShotQuest = 0x1D0;
            public const int QuestTimeLimit = 0x1E8;
            public const int QuestTimeLimit2 = 0x200;
            public const int QuestDailyPlay = 0x218;
            public const int EquipOnAutoQuestStart = 0x230;
            public const int FieldOnAutoQuestStart = 0x248;
            public const int IsEquipAutoQuestStart = 0x260;
            public const int IsFieldAutoQuestStart = 0x278;
            public const int NormalAutoStartQuest = 0x290;
            public const int AutoCompletionAlertQuest = 0x294;
            public const int TimeKeepQuest = 0x2A8;
            public const int QuestCategory = 0x2BC;
            public const int RecentlyUpdatedQuest = 0x2D4;
            public const int RecentlyViewedQuest = 0x2E8;
            public const int RankInfo = 0x2EC;
            public const int QuestExpByLevel = 0x304;
            public const int QuestPerformByDay = 0x31C;
            public const int RankStringInfo = 0x334;
        }

        /// <summary>WORLDITEM struct (sizeof=0x20, array element of CLogin.m_WorldItem ZArray)</summary>
        public static class WorldItem
        {
            public const int Stride = 0x20;
            public const int Id = 0x00;
            public const int NamePtr = 0x04;
            public const int ChannelItemsPtr = 0x1C;
        }

        /// <summary>CHANNELITEM struct (sizeof=0x14, array element within WORLDITEM)</summary>
        public static class ChannelItem
        {
            public const int Stride = 0x14;
            public const int NamePtr = 0x00;
            public const int WorldId = 0x08;
            public const int ChannelId = 0x0C;
            public const int AdultFlag = 0x10;
        }

        public static class CWvsApp
        {
            public const int HWnd = 0x04;
            public const int PcomInitialized = 0x08;
            public const int MainThreadId = 0x0C;
            public const int HHook = 0x10;
            public const int Win9x = 0x14;
            public const int OsVersion = 0x18;
            public const int OsMinorVersion = 0x1C;
            public const int OsBuildNumber = 0x20;
            public const int CsdVersion = 0x24;
            public const int Bit64Info = 0x28;
            public const int UpdateTime = 0x2C;
            public const int FirstUpdate = 0x30;
            public const int CmdLine = 0x34;
            public const int GameStartMode = 0x38;
            public const int AutoConnect = 0x3C;
            public const int ShowAdBalloon = 0x40;
            public const int ExitByTitleEscape = 0x44;
            public const int ZExceptionCode = 0x48;
            public const int ComErrorCode = 0x4C;
            public const int SecurityErrorCode = 0x50;
            public const int TargetVersion = 0x54;
            public const int LastServerIpCheck = 0x58;
            public const int EnabledDx9 = 0x78;
            public const int WindowActive = 0x88;
        }
    }
}
