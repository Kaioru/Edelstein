namespace Edelstein.Common.Gameplay.Social
{
    public enum PartyRequestCode : byte
    {
        LoadParty = 0x0,
        CreateNewParty = 0x1,
        WithdrawParty = 0x2,
        JoinParty = 0x3,
        InviteParty = 0x4,
        KickParty = 0x5,
        ChangePartyBoss = 0x6
    }
}
