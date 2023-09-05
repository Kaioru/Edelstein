using System.Collections;
using Edelstein.Protocol.Utilities.Packets;

namespace Edelstein.Common.Utilities;

public readonly struct Flags : IPacketWritable
{
    private readonly BitArray _bits;

    public Flags(int size)
        => _bits = new BitArray(size);

    public Flags(BitArray bits)
        => _bits = bits;

    public bool SetFlag(int index, bool value = true)
        => _bits[index] = value;

    public bool HasFlag(int index)
        => _bits[index];

    public int[] ToArray()
    {
        var num = _bits.Count / 8 / 4;

        if (_bits.Count % 8 != 0) num++;

        var arr = new int[num];

        _bits.CopyTo(arr, 0);
        return arr;
    }

    public void WriteTo(IPacketWriter writer)
    {
        var arr = ToArray();

        for (var i = arr.Length; i > 0; i--)
            writer.WriteInt(arr[i - 1]);
    }

    public Flags And(Flags b) => new(_bits.And(b._bits));
    public Flags Or(Flags b) => new(_bits.Or(b._bits));
    public Flags Xor(Flags b) => new(_bits.Xor(b._bits));
    public Flags Not() => new(_bits.Not());

    public static Flags operator &(Flags a, Flags b) => a.And(b);
    public static Flags operator |(Flags a, Flags b) => a.Or(b);
    public static Flags operator ^(Flags a, Flags b) => a.Xor(b);
    public static Flags operator ~(Flags a) => a.Not();
}
