namespace Edelstein.Protocol.Gameplay.Game.Conversations.Messages;

public enum ConversationMessageType : byte
{
    Say = 0x0,
    SayImage = 0x1,
    AskYesNo = 0x2,
    AskText = 0x3,
    AskNumber = 0x4,
    AskMenu = 0x5,
    AskQuiz = 0x6,
    AskSpeedQuiz = 0x7,
    AskAvatar = 0x8,
    AskMemberShopAvatar = 0x9,
    AskPet = 0xA,
    AskPetAll = 0xB,
    Script = 0xC,
    AskAccept = 0xD,
    AskBoxText = 0xE,
    AskSlideMenu = 0xF,
    AskCenter = 0x10
}
