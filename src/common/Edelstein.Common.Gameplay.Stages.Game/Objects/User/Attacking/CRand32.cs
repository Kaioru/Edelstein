namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Attacking
{
    public class CRand32
    {
        public int S1 { get; private set; }
        public int S2 { get; private set; }
        public int S3 { get; private set; }

        public CRand32(int s1, int s2, int s3)
        {
            S1 = s1;
            S2 = s2;
            S3 = s3;
        }

        public long Next()
        {
            S1 = ((((S1 >> 6) & 0x3FFFFFF) ^ (S1 << 12)) & 0x1FFF) ^ ((S1 >> 19) & 0x1FFF) ^ (S1 << 12);
            S2 = ((((S2 >> 23) & 0x1FF) ^ (S2 << 4)) & 0x7F) ^ ((S2 >> 25) & 0x7F) ^ (S2 << 4);
            S3 = ((((S3 << 17) ^ ((S3 >> 8) & 0xFFFFFF)) & 0x1FFFFF) ^ (S3 << 17)) ^ ((S3 >> 11) & 0x1FFFFF);

            return (S1 ^ S2 ^ S3) & 0xFFFFFFFFL;
        }

        public void Next(long[] arr)
        {
            for (var i = 0; i < arr.Length; i++)
                arr[i] = Next();
        }
    }
}
