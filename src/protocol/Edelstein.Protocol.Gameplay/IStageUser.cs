using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay;

public interface IStageUser<TStage, TStageUser> : IIdentifiable<int>, IAdapter
    where TStage : IStage<TStage, TStageUser>
    where TStageUser : IStageUser<TStage, TStageUser>
{
    TStage? Stage { get; set; }

    IAccount? Account { get; set; }
    IAccountWorld? AccountWorld { get; set; }
    ICharacter? Character { get; set; }
}
