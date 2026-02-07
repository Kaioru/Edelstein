using System.Text;
using Iced.Intel;
using IcedDecoder = Iced.Intel.Decoder;
using Microsoft.Extensions.Logging;
using Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public partial class RuntimeFunctionDumper
{
    public FunctionDump? DumpFunction(string name, uint address, int maxBytes = 4096, int maxInstructions = 500)
    {
        var bytes = ReadBytes(address, maxBytes);
        if (bytes == null)
        {
            _logger?.LogError("[FuncDump] Failed to read {Count} bytes from 0x{Addr:X8}", maxBytes, address);
            return null;
        }

        _logger?.LogDebug("[FuncDump] Read {Count} bytes from {Name} at 0x{Addr:X8}",
            bytes.Length, name, address);

        var rawBranches = new List<(uint Target, uint SourceIP, FlowControl Flow)>();
        var findings = new List<string>();
        var sb = new StringBuilder();

        var reader = new ByteArrayCodeReader(bytes);
        var decoder = IcedDecoder.Create(32, reader);
        decoder.IP = address;

        var formatter = new IntelFormatter();
        formatter.Options.SpaceAfterOperandSeparator = true;
        formatter.Options.HexPrefix = "0x";
        formatter.Options.HexSuffix = null;
        formatter.Options.UppercaseHex = false;
        var output = new StringOutput();

        var instructionCount = 0;
        var invalidCount = 0;
        var endAddress = (ulong)(address + bytes.Length);
        uint lastDecodedIP = address;
        int lastDecodedLen = 0;

        var provenance = new RegisterProvenance(name, _knownPointers);

        while (decoder.IP < endAddress)
        {
            var instrIP = decoder.IP;
            var instr = decoder.Decode();

            if (instr.IsInvalid)
            {
                invalidCount++;
                var badByte = bytes[(int)(instrIP - address)];
                sb.AppendLine($"  {instrIP:x8}: {badByte:x2}                       db 0x{badByte:x2}");

                if (invalidCount > 10)
                {
                    findings.Add("Stopped after 10+ consecutive invalid instructions — likely VM-protected code");
                    break;
                }
                instructionCount++;
                lastDecodedIP = (uint)instrIP;
                lastDecodedLen = 1;
                continue;
            }

            invalidCount = 0;
            instructionCount++;
            lastDecodedIP = (uint)instrIP;
            lastDecodedLen = instr.Length;

            formatter.Format(instr, output);
            var text = output.ToStringAndReset();

            var instrLen = instr.Length;
            var instrOffset = (int)(instrIP - address);
            var hexParts = new StringBuilder();
            for (var b = 0; b < instrLen && instrOffset + b < bytes.Length; b++)
                hexParts.Append($"{bytes[instrOffset + b]:x2} ");
            var hex = hexParts.ToString().TrimEnd();

            var annotation = AnnotateInstruction(instr, provenance);
            var line = $"  {instrIP:x8}: {hex,-24} {text}";
            if (annotation != null)
                line += $"  ; {annotation}";
            sb.AppendLine(line);

            if (instr.FlowControl is FlowControl.Call or FlowControl.UnconditionalBranch)
            {
                if (instr.Op0Kind is OpKind.NearBranch32 or OpKind.NearBranch16)
                {
                    var target = (uint)instr.NearBranchTarget;
                    rawBranches.Add((target, (uint)instrIP, instr.FlowControl));

                    var targetLabel = _knownPointers.TryGetValue(target, out var kn) ? kn : $"sub_{target:X}";
                    var callType = instr.FlowControl == FlowControl.Call ? "Calls" : "Jumps to";
                    findings.Add($"{callType} 0x{target:X8} ({targetLabel})");
                }
                else if (instr.Op0Kind == OpKind.Register)
                {
                    findings.Add($"Indirect call/jmp via {instr.Op0Register} at 0x{instrIP:X8}");
                }
                else if (instr.Op0Kind == OpKind.Memory)
                {
                    findings.Add($"Indirect call/jmp via memory at 0x{instrIP:X8}");
                }
            }

            AnalyzeMemoryAccess(instr, findings, provenance);

            provenance.ApplyInstruction(instr);

            if (instr.FlowControl == FlowControl.Return)
                break;

            if (instructionCount >= maxInstructions)
            {
                findings.Add($"Stopped after {maxInstructions} instructions (function may be longer)");
                break;
            }
        }

        var funcEnd = lastDecodedIP + (uint)lastDecodedLen;
        var classifiedTargets = new List<CallTarget>();

        foreach (var (target, sourceIP, flow) in rawBranches)
        {
            TargetKind kind;
            if (target >= address && target < funcEnd)
            {
                kind = TargetKind.InternalBranch;
            }
            else if (flow == FlowControl.Call)
            {
                kind = TargetKind.ExternalCall;
            }
            else
            {
                kind = TargetKind.TailJump;
            }
            classifiedTargets.Add(new CallTarget(target, sourceIP, kind));
        }

        if (instructionCount > 5)
        {
            var totalInvalid = 0;
            var reader2 = new ByteArrayCodeReader(bytes);
            var decoder2 = IcedDecoder.Create(32, reader2);
            decoder2.IP = address;
            var total2 = 0;
            while (decoder2.IP < endAddress && total2 < instructionCount)
            {
                var instr2 = decoder2.Decode();
                if (instr2.IsInvalid) totalInvalid++;
                total2++;
            }

            var invalidRatio = totalInvalid / (double)instructionCount;
            if (invalidRatio > 0.3)
                findings.Insert(0, $"HIGH INVALID RATIO ({invalidRatio:P0}) — code may be VM-protected or packed");
        }

        {
            var reader3 = new ByteArrayCodeReader(bytes);
            var decoder3 = IcedDecoder.Create(32, reader3);
            decoder3.IP = address;
            var firstInstr = decoder3.Decode();
            if (!firstInstr.IsInvalid && firstInstr.FlowControl == FlowControl.UnconditionalBranch)
                findings.Insert(0, "Function starts with JMP — likely a trampoline/redirect");
        }

        return new FunctionDump(name, address, funcEnd, bytes.Length, bytes, sb.ToString(), classifiedTargets, findings);
    }

    private string? AnnotateInstruction(Instruction instr, RegisterProvenance provenance)
    {
        for (var i = 0; i < instr.OpCount; i++)
        {
            var kind = instr.GetOpKind(i);

            if (kind == OpKind.Memory)
            {
                var disp = (int)instr.MemoryDisplacement32;

                if (instr.MemoryBase == Register.None && instr.MemoryIndex == Register.None)
                {
                    if (_knownPointers.TryGetValue((uint)disp, out var ptrName))
                        return $"-> {ptrName}";
                }

                if (disp > 0)
                {
                    var accessType = i == 0 && IsStoreInstruction(instr) ? "WRITE" : "READ";

                    if (provenance.TryResolveField(instr.MemoryBase, disp, out var typedLabel))
                        return $"{accessType} {typedLabel} via [{instr.MemoryBase}+0x{disp:X}]";

                    if (IsStackBase(instr.MemoryBase))
                        continue;

                    if (AllKnownOffsets.TryGetValue(disp, out var fieldName))
                    {
                        var label = fieldName;
                        if (fieldName.Contains(" / ", StringComparison.Ordinal))
                            label = $"offset 0x{disp:X} (candidates: {fieldName.Replace(" / ", ", ")})";
                        return $"{accessType} {label} via [{instr.MemoryBase}+0x{disp:X}]";
                    }
                }
            }

            if (kind is OpKind.Immediate32 or OpKind.Immediate32to64)
            {
                var imm = (uint)instr.GetImmediate(i);
                if (_knownPointers.TryGetValue(imm, out var immName))
                    return $"-> {immName}";
            }
        }

        if (instr.FlowControl is FlowControl.Call or FlowControl.UnconditionalBranch or FlowControl.ConditionalBranch)
        {
            if (instr.Op0Kind is OpKind.NearBranch32 or OpKind.NearBranch16)
            {
                var target = (uint)instr.NearBranchTarget;
                if (_knownPointers.TryGetValue(target, out var funcName))
                    return $"-> {funcName}";
            }
        }

        return null;
    }

    private static void AnalyzeMemoryAccess(Instruction instr, List<string> findings, RegisterProvenance provenance)
    {
        for (var i = 0; i < instr.OpCount; i++)
        {
            if (instr.GetOpKind(i) != OpKind.Memory)
                continue;

            var disp = (int)instr.MemoryDisplacement32;
            if (disp <= 0) continue;

            var isWrite = i == 0 && IsStoreInstruction(instr);
            var baseReg = instr.MemoryBase;
            var tag = isWrite ? "[WRITE]" : "[READ]";

            if (provenance.TryResolveField(baseReg, disp, out var typedLabel))
            {
                findings.Add($"{tag} {typedLabel} via [{baseReg}+0x{disp:X}] at 0x{instr.IP:X8}");
                continue;
            }

            if (IsStackBase(baseReg))
                continue;

            if (AllKnownOffsets.TryGetValue(disp, out var fieldName))
            {
                var label = fieldName;
                if (fieldName.Contains(" / ", StringComparison.Ordinal))
                    label = $"offset 0x{disp:X} (candidates: {fieldName.Replace(" / ", ", ")})";
                findings.Add($"{tag} {label} via [{baseReg}+0x{disp:X}] at 0x{instr.IP:X8}");
            }
        }
    }

    private static bool IsStoreInstruction(Instruction instr)
    {
        return instr.Mnemonic switch
        {
            Mnemonic.Cmp => false,
            Mnemonic.Test => false,
            Mnemonic.Bt => false,
            Mnemonic.Btc => true,
            Mnemonic.Btr => true,
            Mnemonic.Bts => true,
            Mnemonic.Bound => false,
            _ => true
        };
    }

    private static bool IsStackBase(Register register)
    {
        return register is Register.ESP or Register.EBP;
    }

    private byte[]? ReadBytes(uint address, int count)
    {
        if (_processHandle == IntPtr.Zero)
            return null;

        var buffer = new byte[count];
        if (Win32Api.ReadProcessMemory(_processHandle, new IntPtr(address), buffer, count, out var bytesRead) && bytesRead > 0)
            return buffer[..bytesRead];

        return null;
    }
}
