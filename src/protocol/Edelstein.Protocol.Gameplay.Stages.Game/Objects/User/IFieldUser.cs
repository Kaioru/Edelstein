using Edelstein.Protocol.Gameplay.Accounts;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

public interface IFieldUser : IFieldLife, IFieldSplitObserver, IFieldController, ICommandContext
{
    IGameStageUser StageUser { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }

    bool IsInstantiated { get; set; }

    IPacket GetSetFieldPacket();
}
