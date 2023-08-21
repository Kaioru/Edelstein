using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Protocol.Gameplay;


public interface IStageUser<TStageUser> : IIdentifiable<int>, IAdapter 
    where TStageUser : IStageUser<TStageUser>
{
    IStage<TStageUser>? Stage { get; set; }

    IAccount? Account { get; set; }
    IAccountWorld? AccountWorld { get; set; }
    ICharacter? Character { get; set; }

    long Key { get; set; }

    bool IsMigrating { get; set; }

    Task Migrate(string serverID, IPacket? packet = null);
}
