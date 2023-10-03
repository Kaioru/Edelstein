using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserShopRechargeRequest(
    IFieldUser User,
    IDialogueNPCShop Shop,
    short Slot
);
