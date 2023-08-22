using Edelstein.Protocol.Gameplay.Models.Accounts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Protocol.Gameplay.Game.Objects.User;

public interface IFieldUser:
    IFieldLife<IFieldUserMovePath, IFieldUserMoveAction>,
    IFieldSplitObserver, IFieldController
{
    IGameStageUser StageUser { get; }

    IAccount Account { get; }
    IAccountWorld AccountWorld { get; }
    ICharacter Character { get; }

    bool IsInstantiated { get; set; }

    IPacket GetSetFieldPacket();
    
    Task ModifyInventory(Action<IModifyInventoryGroupContext>? action = null, bool exclRequest = false);
}
