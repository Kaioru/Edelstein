using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages;

public interface IStageUser<TStageUser> : IIdentifiable<int>, IAdapter where TStageUser : IStageUser<TStageUser>
{
    IStage<TStageUser>? Stage { get; set; }

    IAccount? Account { get; set; }
    IAccountWorld? AccountWorld { get; set; }
    ICharacter? Character { get; set; }
}
