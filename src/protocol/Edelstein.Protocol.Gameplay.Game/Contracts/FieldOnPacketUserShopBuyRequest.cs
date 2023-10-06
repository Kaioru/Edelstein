using Edelstein.Protocol.Gameplay.Game.Dialogues;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Game.Contracts;

public record FieldOnPacketUserShopBuyRequest(
    IFieldUser User,
    IDialogueNPCShop Shop,
    short Position,
    int TemplateID,
    short Count
);
