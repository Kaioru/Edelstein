namespace Edelstein.Plugin.Rue.Commands;

/// <summary>
/// Utility for normalizing search keys by trimming whitespace,
/// removing control characters, and optionally lowercasing.
/// </summary>
public static class StringNormalizer
{
    private const int MaxKeyLength = 256;

    /// <summary>
    /// Normalizes a key by trimming whitespace and removing control characters.
    /// When lowercase=true, also lowercases in a single allocation for efficiency.
    /// </summary>
    /// <returns>Normalized string, or null if input is empty/whitespace-only.</returns>
    public static string? NormalizeKey(string key, bool lowercase = false)
    {
        if (string.IsNullOrEmpty(key)) return null;

        var start = 0;
        var end = key.Length - 1;

        while (start <= end && char.IsWhiteSpace(key[start])) start++;
        while (end >= start && char.IsWhiteSpace(key[end])) end--;

        if (start > end) return null;

        var length = Math.Min(end - start + 1, MaxKeyLength);
        var slice = key.AsSpan(start, length);

        // Check what transformations are needed
        var needsCleanup = false;
        var needsLowercase = false;
        for (var i = 0; i < slice.Length; i++)
        {
            var c = slice[i];
            if (c == '\0' || (char.IsControl(c) && !char.IsWhiteSpace(c)))
                needsCleanup = true;
            else if (lowercase && char.IsUpper(c))
                needsLowercase = true;
        }

        // Fast path: no transformations needed
        if (!needsCleanup && !needsLowercase)
        {
            if (start == 0 && length == key.Length) return key;
            return key.Substring(start, length);
        }

        // Medium path: just lowercase, no cleanup - use string.Create
        if (!needsCleanup && lowercase)
        {
            return string.Create(length, (key, start), static (dest, state) =>
            {
                var (src, srcStart) = state;
                for (var i = 0; i < dest.Length; i++)
                    dest[i] = char.ToLowerInvariant(src[srcStart + i]);
            });
        }

        // Slow path: needs cleanup - use stackalloc
        Span<char> buffer = stackalloc char[length];
        var outLength = 0;

        for (var i = 0; i < slice.Length; i++)
        {
            var c = slice[i];
            if (c == '\0') continue;
            if (char.IsControl(c) && !char.IsWhiteSpace(c)) continue;
            buffer[outLength++] = lowercase ? char.ToLowerInvariant(c) : c;
        }

        return outLength == 0 ? null : new string(buffer[..outLength]);
    }
}
