namespace Edelstein.Common.Gameplay.Social;

public enum PartyResultOperations
{
    LoadPartyDone = 0x7,

    CreateNewPartyDone = 0x8,
    CreateNewPartyAlreayJoined = 0x9,
    CreateNewPartyBeginner = 0xA,
    CreateNewPartyUnknown = 0xB,

    WithdrawPartyDone = 0xC,
    WithdrawPartyNotJoined = 0xD,
    WithdrawPartyUnknown = 0xE,

    JoinPartyDone = 0xF,
    JoinPartyDone2 = 0x10,
    JoinPartyAlreadyJoined = 0x11,
    JoinPartyAlreadyFull = 0x12,
    JoinPartyOverDesiredSize = 0x13,
    JoinPartyUnknownUser = 0x14,
    JoinPartyUnknown = 0x15,

    InvitePartySent = 0x16,
    InvitePartyBlockedUser = 0x17,
    InvitePartyAlreadyInvited = 0x18,
    InvitePartyAlreadyInvitedByInviter = 0x19,

    InvitePartyRejected = 0x1A,
    InvitePartyAccepted = 0x1B,

    KickPartyDone = 0x1C,
    KickPartyFieldLimit = 0x1D,
    KickPartyUnknown = 0x1E,

    ChangePartyBossDone = 0x1F,
    ChangePartyBossNotSameField = 0x20,
    ChangePartyBossNoMemberInSameField = 0x21,
    ChangePartyBossNotSameChannel = 0x22,
    ChangePartyBossUnknown = 0x23,

    AdminCannotCreate = 0x24,
    AdminCannotInvite = 0x25,

    UserMigration = 0x26,
    ChangeLevelOrJob = 0x27,
    SuccessToSelectPQReward = 0x28,
    FailToSelectPQReward = 0x29,
    ReceivePQReward = 0x2A,
    FailToRequestPQReward = 0x2B,
    CanNotInThisField = 0x2C,
    ServerMsg = 0x2D,
    
    TownPortalChanged = 0x2E,
    OpenGate = 0x2F,
}
