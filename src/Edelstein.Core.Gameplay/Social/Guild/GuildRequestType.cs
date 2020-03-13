namespace Edelstein.Core.Gameplay.Social.Guild
{
    public enum GuildRequestType : byte
    {
        LoadGuild = 0x0,
        InputGuildName = 0x1,
        CheckGuildName = 0x2,
        CreateGuildAgree = 0x3,
        CreateNewGuild = 0x4,
        InviteGuild = 0x5,
        JoinGuild = 0x6,
        WithdrawGuild = 0x7,
        KickGuild = 0x8,
        RemoveGuild = 0x9,
        IncMaxMemberNum = 0xA,
        ChangeLevel = 0xB,
        ChangeJob = 0xC,
        SetGradeName = 0xD,
        SetMemberGrade = 0xE,
        SetMark = 0xF,
        SetNotice = 0x10,
        InputMark = 0x11,
        CheckQuestWaiting = 0x12,
        CheckQuestWaiting2 = 0x13,
        InsertQuestWaiting = 0x14,
        CancelQuestWaiting = 0x15,
        RemoveQuestCompleteGuild = 0x16,
        IncPoint = 0x17,
        IncCommitment = 0x18,
        SetQuestTime = 0x19,
        ShowGuildRanking = 0x1A,
        SetSkill = 0x1B
    }
}