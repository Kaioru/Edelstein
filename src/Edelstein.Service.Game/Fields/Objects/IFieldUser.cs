using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Core.Entities.Characters;
using Edelstein.Core.Gameplay.Social.Guild;
using Edelstein.Core.Gameplay.Social.Memo;
using Edelstein.Core.Gameplay.Social.Party;
using Edelstein.Core.Network.Packets;

namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldUser : IFieldLife
    {
        GameService Service { get; }
        GameServiceAdapter Adapter { get; }
        Character Character { get; }
        bool IsInstantiated { get; set; }

        IDictionary<int, ISocialMemo> Memos { get; }
        ISocialParty? Party { get; set; }
        ISocialGuild? Guild { get; set; }

        IFieldSplit[] Watching { get; }
        ICollection<IFieldControlled> Controlling { get; }

        IFieldObj GetWatchedObject(int id);
        T GetWatchedObject<T>(int id) where T : IFieldObj;
        IEnumerable<IFieldObj> GetWatchedObjects();
        IEnumerable<T> GetWatchedObjects<T>() where T : IFieldObj;

        IPacket GetSetFieldPacket();

        Task SendPacket(IPacket packet);
    }
}