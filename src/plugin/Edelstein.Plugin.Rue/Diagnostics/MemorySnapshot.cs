using System.Text;
using Edelstein.Plugin.Rue.ClientAnalysis;

namespace Edelstein.Plugin.Rue.Diagnostics;

public record MemorySnapshot(
    DateTime Timestamp,
    uint? AccountId,
    int? WorldId,
    int? ChannelId,
    int? CharacterCount,
    int? SlotCount,
    int? ChannelNameArrayPtr,
    int? AdultChannelArrayPtr,
    bool? RequestSent,
    int? LoginStep,
    int? StepChanging,
    int? CharSelected,
    byte? LoginOpt,
    bool ChannelSelectExists,
    int? SelectedChannel,
    int? WorldItemPtr,
    bool ConnectionDlgExists,
    bool WorldSelectExists,
    int? WorldIdx)
{
    public string Describe()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== Client Memory State ===");
        sb.AppendLine($"Timestamp: {Timestamp:HH:mm:ss.fff}");
        sb.AppendLine();
        sb.AppendLine("[CWvsContext]");
        sb.AppendLine($"  m_dwAccountId: {AccountId?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nWorldID: {WorldId?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nChannelID: {ChannelId?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nCharacterCount: {CharacterCount?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nSlotCount: {SlotCount?.ToString() ?? "null"}");
        sb.AppendLine($"  m_aChannelName.a: {FormatPtr(ChannelNameArrayPtr)}");
        sb.AppendLine($"  m_aAdultChannel.a: {FormatPtr(AdultChannelArrayPtr)}");
        sb.AppendLine();
        sb.AppendLine("[CLogin]");
        sb.AppendLine($"  m_bRequestSent: {RequestSent?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nLoginStep: {LoginStep?.ToString() ?? "null"} ({DescribeLoginStep(LoginStep)})");
        sb.AppendLine($"  m_tStepChanging: {StepChanging?.ToString() ?? "null"}");
        sb.AppendLine($"  m_nCharSelected: {CharSelected?.ToString() ?? "null"}");
        sb.AppendLine($"  m_bLoginOpt: {LoginOpt?.ToString() ?? "null"}");
        sb.AppendLine();
        sb.AppendLine("[CUIChannelSelect]");
        sb.AppendLine($"  Exists: {ChannelSelectExists}");
        sb.AppendLine($"  m_nSelect: {SelectedChannel?.ToString() ?? "null"}");
        sb.AppendLine($"  m_pWorldItem: {FormatPtr(WorldItemPtr)}");
        sb.AppendLine($"  m_pConnectionDlg: {(ConnectionDlgExists ? "exists" : "null")}");
        sb.AppendLine();
        sb.AppendLine("[CUIWorldSelect]");
        sb.AppendLine($"  Exists: {WorldSelectExists}");
        sb.AppendLine($"  m_nWorldIdx: {WorldIdx?.ToString() ?? "null"}");
        return sb.ToString();
    }

    private static string FormatPtr(int? ptr)
        => ptr.HasValue ? $"0x{ptr.Value:X8}" : "null";

    private static string DescribeLoginStep(int? step) =>
        step.HasValue && Enum.IsDefined(typeof(LoginStep), step.Value)
            ? ((LoginStep)step.Value).ToString()
            : "Unknown";
}
