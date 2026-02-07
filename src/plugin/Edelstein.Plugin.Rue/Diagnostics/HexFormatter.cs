using System.Text;

namespace Edelstein.Plugin.Rue.Diagnostics;

public static class HexFormatter
{
    private static ReadOnlySpan<char> HexDigits => "0123456789ABCDEF";

    public static string FormatHexDump(byte[] buffer, int bytesPerLine = 16)
        => FormatHexDump(buffer.AsSpan(), bytesPerLine);

    public static string FormatHexDump(ReadOnlySpan<byte> buffer, int bytesPerLine = 16)
    {
        var sb = new StringBuilder();
        AppendHexDump(sb, buffer, baseAddress: 0, indent: "  ", bytesPerLine: bytesPerLine);
        return sb.ToString();
    }

    public static void AppendHexDump(
        StringBuilder sb, byte[] bytes, uint baseAddress,
        int maxBytes = int.MaxValue, string indent = "    ", int bytesPerLine = 16)
        => AppendHexDump(sb, bytes.AsSpan(), baseAddress, maxBytes, indent, bytesPerLine);

    public static void AppendHexDump(
        StringBuilder sb, ReadOnlySpan<byte> buffer, uint baseAddress,
        int maxBytes = int.MaxValue, string indent = "    ", int bytesPerLine = 16)
    {
        var len = Math.Min(buffer.Length, maxBytes);
        var slice = buffer[..len];

        Span<char> hexBuffer = stackalloc char[(bytesPerLine * 3) + 1];
        Span<char> asciiBuffer = stackalloc char[bytesPerLine];

        for (var i = 0; i < slice.Length; i += bytesPerLine)
        {
            sb.Append(indent);
            sb.Append((baseAddress + (uint)i).ToString("x8"));
            sb.Append(": ");

            var lineBytes = slice.Slice(i, Math.Min(bytesPerLine, slice.Length - i));
            FillHexBuffer(hexBuffer, lineBytes, bytesPerLine);
            sb.Append(hexBuffer);

            sb.Append(" |");
            var asciiCount = FillAsciiBuffer(asciiBuffer, lineBytes, bytesPerLine);
            sb.Append(asciiBuffer[..asciiCount]);

            sb.AppendLine("|");
        }

        if (buffer.Length > maxBytes)
            sb.AppendLine($"{indent}... ({buffer.Length - maxBytes} more bytes)");
    }

    public static string FormatAsciiDump(byte[] buffer, int bytesPerLine = 16)
        => FormatAsciiDump(buffer.AsSpan(), bytesPerLine);

    public static string FormatAsciiDump(ReadOnlySpan<byte> buffer, int bytesPerLine = 16)
    {
        if (buffer.Length == 0) return "(empty)";

        var sb = new StringBuilder();
        Span<char> hexBuffer = stackalloc char[(bytesPerLine * 3) + 1];
        Span<char> asciiBuffer = stackalloc char[bytesPerLine];
        for (var i = 0; i < buffer.Length; i += bytesPerLine)
        {
            sb.Append(i.ToString("X4"));
            sb.Append("  ");

            var lineBytes = buffer.Slice(i, Math.Min(bytesPerLine, buffer.Length - i));
            FillHexBuffer(hexBuffer, lineBytes, bytesPerLine);
            sb.Append(hexBuffer);

            sb.Append(" |");
            var asciiCount = FillAsciiBuffer(asciiBuffer, lineBytes, bytesPerLine);
            sb.Append(asciiBuffer[..asciiCount]);

            sb.AppendLine("|");
        }

        return sb.ToString();
    }

    private static void FillHexBuffer(Span<char> buffer, ReadOnlySpan<byte> lineBytes, int bytesPerLine)
    {
        buffer.Fill(' ');

        for (var j = 0; j < bytesPerLine; j++)
        {
            if (j >= lineBytes.Length) continue;

            var b = lineBytes[j];
            var offset = (j * 3) + (j > 7 ? 1 : 0);
            buffer[offset] = HexDigits[b >> 4];
            buffer[offset + 1] = HexDigits[b & 0xF];
            buffer[offset + 2] = ' ';
        }
    }

    private static int FillAsciiBuffer(Span<char> buffer, ReadOnlySpan<byte> lineBytes, int bytesPerLine)
    {
        var count = Math.Min(bytesPerLine, lineBytes.Length);

        for (var j = 0; j < count; j++)
        {
            var b = lineBytes[j];
            buffer[j] = b is >= 32 and < 127 ? (char)b : '.';
        }

        return count;
    }
}
