using System;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Conversations
{
    [Flags]
    public enum ConversationSpeakerFlags : byte
    {
        NoESC = 0x1,
        NPCReplacedByUser = 0x2,
        NPCReplacedByNPC = 0x4,
        FlipImage = 0x8
    }
}
