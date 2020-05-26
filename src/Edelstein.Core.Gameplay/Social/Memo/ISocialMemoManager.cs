using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Characters;

namespace Edelstein.Core.Gameplay.Social.Memo
{
    public interface ISocialMemoManager
    {
        Task<ICollection<ISocialMemo>> Load(Character character);
        Task Send(int characterID, string sender, string content, byte flag = 0, DateTime? dateSent = null);
        Task Delete(ISocialMemo memo);
        Task BulkDelete(ICollection<ISocialMemo> memos);
    }
}