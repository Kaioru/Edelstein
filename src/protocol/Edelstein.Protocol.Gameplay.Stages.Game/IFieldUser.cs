using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Buffers.Bytes;

namespace Edelstein.Protocol.Gameplay.Stages.Game;

public interface IFieldUser : IFieldLife, IFieldSplitObserver, IFieldController
{
    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }

    bool IsInstantiated { get; set; }

    IPacket GetSetFieldPacket();
}
