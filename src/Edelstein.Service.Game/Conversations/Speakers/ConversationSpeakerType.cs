using System;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    [Flags]
    public enum ConversationSpeakerType : byte
    {
        NoESC = 0x1,
        NPCReplacedByUser = 0x2,
        NPCReplacedByNPC = 0x4,
        FlipImage = 0x8
    }
}