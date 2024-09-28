using System.Collections;

namespace Edelstein.Protocol.Utilities;

public readonly struct Flags
{
    private readonly BitArray _bits;

    public Flags(int size)
        => _bits = new BitArray(size);

    public Flags(BitArray bits)
        => _bits = bits;

    public void SetFlag(int index, bool value = true)
        => _bits.Set(index, value);

    public bool HasFlag(int index)
        => _bits.Get(index);

    public int[] ToArray()
    {
        var num = _bits.Count / 8 / 4;

        if (_bits.Count % 8 != 0) num++;

        var arr = new int[num];

        _bits.CopyTo(arr, 0);
        return arr;
    }

    private Flags And(Flags b) => new(_bits.And(b._bits));
    private Flags Or(Flags b) => new(_bits.Or(b._bits));
    private Flags Xor(Flags b) => new(_bits.Xor(b._bits));
    private Flags Not() => new(_bits.Not());

    public static Flags operator &(Flags a, Flags b) => a.And(b);
    public static Flags operator |(Flags a, Flags b) => a.Or(b);
    public static Flags operator ^(Flags a, Flags b) => a.Xor(b);
    public static Flags operator ~(Flags a) => a.Not();
}
