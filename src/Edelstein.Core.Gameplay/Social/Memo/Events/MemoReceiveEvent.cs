namespace Edelstein.Core.Gameplay.Social.Memo.Events
{
    public class MemoReceiveEvent
    {
        public int CharacterID { get; }
        public Entities.Social.Memo Memo { get; }

        public MemoReceiveEvent(int characterID, Entities.Social.Memo memo)
        {
            CharacterID = characterID;
            Memo = memo;
        }
    }
}