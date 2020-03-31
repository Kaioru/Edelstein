using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public class SocialMemo : ISocialMemo
    {
        private readonly ISocialMemoManager _manager;
        private readonly Entities.Social.Memo _memo;

        public int ID => _memo.ID;
        public int CharacterID => _memo.CharacterID;
        public string Sender => _memo.Sender;
        public string Content => _memo.Content;
        public DateTime DateSent => _memo.DateSent;
        public byte Flag => _memo.Flag;

        public SocialMemo(ISocialMemoManager manager, Entities.Social.Memo memo)
        {
            _manager = manager;
            _memo = memo;
        }

        public Task Read()
            => _manager.Delete(this);
    }
}