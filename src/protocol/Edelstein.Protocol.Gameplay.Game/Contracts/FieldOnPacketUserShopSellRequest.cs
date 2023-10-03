using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserShopSellRequest(
    IFieldUser User,
    IDialogueNPCShop Shop,
    short Slot,
    int TemplateID,
    short Count
);
