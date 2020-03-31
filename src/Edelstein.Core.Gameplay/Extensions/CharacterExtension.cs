using Edelstein.Entities.Characters;

namespace Edelstein.Core.Gameplay.Extensions
{
    public static class CharacterExtensions
    {
        public static byte GetExtendSP(this Character c, byte jobLevel)
            => c.ExtendSP.TryGetValue(jobLevel, out var sp)
                ? sp
                : (byte) 0;

        public static void SetExtendSP(this Character c, byte jobLevel, byte sp)
            => c.ExtendSP[jobLevel] = sp;
    }
}