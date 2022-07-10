using System;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Types
{
    [Flags]
    public enum WhisperFlags : byte
    {
        Location = 0x1,
        Whisper = 0x2,
        Request = 0x4,
        Result = 0x8,
        Receive = 0x10,
        Blocked = 0x20,
        Location_F = 0x40,
        Manager = 0x80
    }
}
