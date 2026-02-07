using System.Buffers.Binary;
using System.Text;
using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Plugin.Rue.ClientAnalysis;

namespace Edelstein.Plugin.Rue.Diagnostics;

/// <summary>
/// Client crash call types sent in ClientDumpLog packets.
/// Indicates which subsystem detected the crash.
/// </summary>
public enum CrashCallType : short
{
    General = 0,
    PacketHandler = 1,
    Network = 2,
    Decode = 3,
}

public static class CrashAnalyzer
{
    private const byte ResultSuccess = 0x00;
    private const byte ResultEndMarker = 0xFF;
    private const int MinimumCrashBufferLength = 2;
    private const int MinimumCharacterNamesBufferLength = 6;
    private const int CharacterNamesInitialOffset = 2;
    private const int CharacterNamesEntryPrefixBytes = 4;
    private const int CharacterNamesLengthBytes = 2;
    private const int CharacterNameMaxLength = 13;
    private const int CharacterNamesMaxCount = 10;
    private const int CharacterNamesEntryStride = 150;


    public static void ParseCrashBuffer(short type, ReadOnlySpan<byte> buffer, Dictionary<string, object?> details)
    {
        if (buffer.Length < MinimumCrashBufferLength) return;

        try
        {
            switch (type)
            {
                case (short)PacketSendOperations.SelectWorldResult:
                    if (buffer.Length >= 1)
                    {
                        details["parsedResult"] = buffer[0];
                        details["parsedResultName"] = buffer[0] == ResultSuccess ? "SUCCESS" : $"FAIL({buffer[0]})";
                        if (buffer[0] == ResultSuccess && buffer.Length >= 2)
                        {
                            details["parsedCharCount"] = buffer[1];
                            var charNames = ExtractCharacterNames(buffer);
                            if (charNames.Count > 0)
                                details["parsedCharNames"] = string.Join(", ", charNames);
                        }
                    }
                    break;

                case (short)PacketSendOperations.CheckUserLimitResult:
                    if (buffer.Length >= 2)
                    {
                        details["parsedOverUserLimit"] = buffer[0];
                        details["parsedPopulateLevel"] = buffer[1];
                    }
                    break;

                case (short)PacketSendOperations.WorldInformation:
                    if (buffer.Length >= 1)
                    {
                        details["parsedWorldId"] = buffer[0];
                        if (buffer[0] == ResultEndMarker)
                            details["parsedNote"] = "End marker (0xFF)";
                    }
                    break;

                case (short)PacketSendOperations.CheckPasswordResult:
                    if (buffer.Length >= 1)
                    {
                        details["parsedResult"] = buffer[0];
                        details["parsedResultName"] = GetLoginResultName(buffer[0]);
                    }
                    break;
            }
        }
        catch
        {
        }
    }

    public static string AnalyzeCrash(short type, int errorCode)
    {
        var sb = new StringBuilder();

        switch (type)
        {
            case (short)PacketSendOperations.SelectWorldResult:
                sb.AppendLine("  CRASH IN: CLogin::OnSelectWorldResult");
                sb.AppendLine("  POSSIBLE CAUSES:");
                sb.AppendLine("    1. CWvsContext->m_nWorldID not set before SelectWorldResult");
                sb.AppendLine("    2. CWvsContext->m_nChannelID not set before SelectWorldResult");
                sb.AppendLine("    3. CLogin->m_bRequestSent not set to 1");
                sb.AppendLine("    4. CLogin->m_tStepChanging was non-zero (client was transitioning)");
                sb.AppendLine("    5. Character parsing failed (malformed packet data)");
                sb.AppendLine("    6. CUIChannelSelect->m_pWorldItem is NULL or has garbage worldID");
                sb.AppendLine("    7. CUIWorldSelect->m_nWorldIdx was garbage when CUIChannelSelect was created");
                break;

            case (short)PacketSendOperations.CheckUserLimitResult:
                sb.AppendLine("  CRASH IN: CLogin::OnCheckUserLimitResult → CUIWorldSelect::UserLimitResult");
                sb.AppendLine("  POSSIBLE CAUSES:");
                sb.AppendLine("    1. CUIWorldSelect does not exist (WorldInformation not processed yet)");
                sb.AppendLine("    2. CheckUserLimitResult sent before WorldInformation");
                sb.AppendLine("    3. CUIWorldSelect->m_nWorldIdx not set (garbage value)");
                sb.AppendLine("  FIX: Wait for CUIWorldSelect to exist before sending CheckUserLimitResult");
                sb.AppendLine("       AND set CUIWorldSelect->m_nWorldIdx before CheckUserLimitResult");
                break;

            case (short)PacketSendOperations.WorldInformation:
                sb.AppendLine("  CRASH IN: WorldInformation processing");
                sb.AppendLine("  POSSIBLE CAUSES:");
                sb.AppendLine("    1. Invalid world data in packet");
                sb.AppendLine("    2. Too many worlds/channels");
                sb.AppendLine("    3. Malformed world name/channel data");
                break;

            case (short)PacketSendOperations.SelectCharacterResult:
                sb.AppendLine("  CRASH IN: CLogin::OnSelectCharacterResult");
                sb.AppendLine("  POSSIBLE CAUSES:");
                sb.AppendLine("    1. No character selected (m_nCharSelected invalid)");
                sb.AppendLine("    2. Character data corrupted");
                sb.AppendLine("    3. Migration data malformed");
                break;

            case (short)PacketSendOperations.CheckPasswordResult:
                sb.AppendLine("  CRASH IN: CLogin::OnCheckPasswordResult");
                sb.AppendLine("  POSSIBLE CAUSES:");
                sb.AppendLine("    1. Invalid login result code");
                sb.AppendLine("    2. Account data malformed");
                break;

            default:
                sb.AppendLine($"  CRASH IN: Packet handler for opcode 0x{type:X2}");
                sb.AppendLine("  Unable to determine specific cause.");
                break;
        }

        sb.AppendLine();
        sb.AppendLine("  ERROR CODE ANALYSIS:");
        if (errorCode == 0)
        {
            sb.AppendLine("    0x00000000 - Likely NULL pointer dereference");
            sb.AppendLine("    A pointer was NULL when the code tried to read/write through it.");
        }
        else
        {
            var errorName = GetNTStatusName(errorCode);
            sb.AppendLine($"    0x{errorCode:X8} - {errorName}");
            sb.AppendLine(GetNTStatusDescription(errorCode));
        }

        return sb.ToString();
    }

    private static List<string> ExtractCharacterNames(ReadOnlySpan<byte> buffer)
    {
        var names = new List<string>();
        if (buffer.Length < MinimumCharacterNamesBufferLength) return names;

        var offset = CharacterNamesInitialOffset;
        Span<char> charBuffer = stackalloc char[CharacterNameMaxLength];
        while (offset + MinimumCharacterNamesBufferLength < buffer.Length && names.Count < CharacterNamesMaxCount)
        {
            offset += CharacterNamesEntryPrefixBytes;

            if (offset + CharacterNamesLengthBytes > buffer.Length) break;
            var lengthSlice = buffer[offset..(offset + CharacterNamesLengthBytes)];
            var strLen = (int)BinaryPrimitives.ReadUInt16LittleEndian(lengthSlice);

            if (strLen > 0 && strLen <= CharacterNameMaxLength && offset + CharacterNamesLengthBytes + strLen <= buffer.Length)
            {
                var nameSlice = buffer[(offset + CharacterNamesLengthBytes)..(offset + CharacterNamesLengthBytes + strLen)];
                var charsWritten = Encoding.ASCII.GetChars(nameSlice, charBuffer);
                var name = new string(charBuffer[..charsWritten]);

                if (!string.IsNullOrWhiteSpace(name) && name.All(c => char.IsLetterOrDigit(c)))
                    names.Add(name);

                offset += CharacterNamesLengthBytes + strLen;
            }
            else
            {
                break;
            }

            offset += CharacterNamesEntryStride;
        }

        return names;
    }

    private static string GetLoginResultName(byte resultCode)
    {
        var result = (LoginResult)resultCode;
        return Enum.IsDefined(typeof(LoginResult), result) ? result.ToString() : $"UNKNOWN({resultCode})";
    }

    public static string GetNTStatusName(int errorCode)
    {
        var status = (Win32Api.NtStatus)(uint)errorCode;
        return status switch
        {
            Win32Api.NtStatus.AccessViolation => "STATUS_ACCESS_VIOLATION",
            Win32Api.NtStatus.InPageError => "STATUS_IN_PAGE_ERROR",
            Win32Api.NtStatus.IllegalInstruction => "STATUS_ILLEGAL_INSTRUCTION",
            Win32Api.NtStatus.NonContinuableException => "STATUS_NONCONTINUABLE_EXCEPTION",
            Win32Api.NtStatus.StackOverflow => "STATUS_STACK_OVERFLOW",
            Win32Api.NtStatus.IntegerDivideByZero => "STATUS_INTEGER_DIVIDE_BY_ZERO",
            Win32Api.NtStatus.IntegerOverflow => "STATUS_INTEGER_OVERFLOW",
            Win32Api.NtStatus.ArrayBoundsExceeded => "STATUS_ARRAY_BOUNDS_EXCEEDED",
            Win32Api.NtStatus.FloatDenormalOperand => "STATUS_FLOAT_DENORMAL_OPERAND",
            Win32Api.NtStatus.FloatDivideByZero => "STATUS_FLOAT_DIVIDE_BY_ZERO",
            Win32Api.NtStatus.FloatInexactResult => "STATUS_FLOAT_INEXACT_RESULT",
            Win32Api.NtStatus.FloatInvalidOperation => "STATUS_FLOAT_INVALID_OPERATION",
            Win32Api.NtStatus.FloatOverflow => "STATUS_FLOAT_OVERFLOW",
            Win32Api.NtStatus.FloatStackCheck => "STATUS_FLOAT_STACK_CHECK",
            Win32Api.NtStatus.FloatUnderflow => "STATUS_FLOAT_UNDERFLOW",
            Win32Api.NtStatus.PrivilegedInstruction => "STATUS_PRIVILEGED_INSTRUCTION",
            Win32Api.NtStatus.GuardPageViolation => "STATUS_GUARD_PAGE_VIOLATION",
            Win32Api.NtStatus.DatatypeMisalignment => "STATUS_DATATYPE_MISALIGNMENT",
            Win32Api.NtStatus.Breakpoint => "STATUS_BREAKPOINT",
            Win32Api.NtStatus.SingleStep => "STATUS_SINGLE_STEP",
            Win32Api.NtStatus.CppException => "C++ EXCEPTION (throw)",
            _ => $"UNKNOWN (0x{(uint)errorCode:X8})"
        };
    }

    private static string GetNTStatusDescription(int errorCode)
    {
        var status = (Win32Api.NtStatus)(uint)errorCode;
        return status switch
        {
            Win32Api.NtStatus.AccessViolation => "    The code tried to read or write to an invalid memory address.\n" +
                          "    Common causes: NULL pointer, freed memory, stack corruption, bad cast.",
            Win32Api.NtStatus.StackOverflow => "    The call stack exceeded its size limit.\n" +
                          "    Common causes: Infinite recursion, excessive local variables.",
            Win32Api.NtStatus.IntegerDivideByZero => "    Integer division by zero.",
            Win32Api.NtStatus.IllegalInstruction => "    CPU encountered an invalid instruction.\n" +
                          "    Common causes: Jump to bad address, corrupted function pointer.",
            Win32Api.NtStatus.NonContinuableException => "    A non-continuable exception was raised.",
            Win32Api.NtStatus.Breakpoint => "    A software breakpoint was hit (INT3 instruction).\n" +
                          "    This might be a debug assertion or intentional crash.",
            Win32Api.NtStatus.CppException => "    A C++ exception was thrown and not caught.\n" +
                          "    The code threw an exception that propagated to the top level.",
            _ => "    No additional description available for this error code."
        };
    }
}
