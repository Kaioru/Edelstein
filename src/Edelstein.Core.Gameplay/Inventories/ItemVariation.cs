using Edelstein.Core.Gameplay.Utils;

namespace Edelstein.Core.Gameplay.Inventories
{
    public class ItemVariation
    {
        private readonly Rand32 _rand;
        private readonly ItemVariationType _type;

        public ItemVariation(Rand32 rand, ItemVariationType type)
        {
            _rand = rand;
            _type = type;
        }

        public int Get(int a)
        {
            if (a <= 0) return a;
            if (_type != ItemVariationType.Gachapon)
            {
                var v9 = a / 10 + 1;
                if (v9 >= 5) v9 = 5;
                var v10 = 1 << (v9 + 2);
                var v11 = v10 > 0 ? _rand.Random() % v10 : _rand.Random();
                var v13 = v11 >> 1;
                var v14 = ((v13 >> 3) & 1)
                          + ((v13 >> 2) & 1)
                          + ((v13 >> 1) & 1)
                          + (v13 & 1)
                          + (v11 & 1)
                          - 2
                          + ((v13 >> 4) & 1)
                          + ((v13 >> 5) & 1);
                if (v14 <= 0)
                    v14 = 0;
                if (_type == ItemVariationType.Normal)
                {
                    var v15 = (_rand.Random() & 1) == 0;
                    if (v15) return a - v14;
                }
                else
                {
                    var v16 = _rand.Random() % 10 < (_type == ItemVariationType.Better ? 3 : 1);
                    if (_type != ItemVariationType.Great)
                        return a;
                    if (v16) return a;
                }

                return a + v14;
            }

            var v3 = a / 5 + 1;
            if (v3 >= 7) v3 = 7;
            var v4 = 1 << (v3 + 2);
            var v17 = v3 + 2;
            var v5 = v4 > 0 ? _rand.Random() % v4 : _rand.Random();
            var v7 = -2;

            if (v17 > 0)
            {
                for (var i = 0; i < v17; i++)
                {
                    v7 += v5 & 1;
                    v5 >>= 1;
                }
            }

            var v8 = (_rand.Random() & 1) > 0 ? a + v7 : a - v7;
            if (v8 <= 0) v8 = 0;
            return v8;
        }
    }
}