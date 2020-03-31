using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public interface ISocialMemo
    {
        int ID { get; }
        int CharacterID { get; }
        string Sender { get; }
        string Content { get; }
        DateTime DateSent { get; }
        byte Flag { get; }

        Task Read();
    }
}