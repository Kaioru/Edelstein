using System;

namespace Edelstein.Protocol.Gameplay.Game.Dialogs.Conversations.Speakers;

[Flags]
public enum ConversationSpeakerParam : byte
{
    NoESC = 0x1,
    NPCReplacedByUser = 0x2,
    NPCReplacedByNPC = 0x4,
    FlipImage = 0x8
}
