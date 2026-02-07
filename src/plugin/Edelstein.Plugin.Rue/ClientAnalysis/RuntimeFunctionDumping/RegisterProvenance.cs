using Iced.Intel;

namespace Edelstein.Plugin.Rue.ClientAnalysis.RuntimeFunctionDumping;

public sealed class RegisterProvenance
{
    private readonly Dictionary<Register, RegisterTypeInfo> _registers = [];
    private readonly Dictionary<uint, string> _singletonTypes = [];

    public RegisterProvenance(string functionName, IReadOnlyDictionary<uint, string> knownPointers)
    {
        foreach (var kv in knownPointers)
        {
            var typeName = TryExtractTypeName(kv.Value);
            if (typeName != null && StructFieldRegistry.ContainsType(typeName))
                _singletonTypes[kv.Key] = typeName;
        }

        var rootType = TryExtractTypeName(functionName);
        if (rootType != null && StructFieldRegistry.ContainsType(rootType))
            SetStruct(Register.ECX, rootType, 0);
    }

    public bool TryResolveField(Register baseReg, int disp, out string label)
    {
        if (_registers.TryGetValue(baseReg, out var info) && info.Kind == RegisterValueKind.StructBase)
        {
            var offset = info.BaseOffset + disp;
            if (StructFieldRegistry.TryGetField(info.TypeName, offset, out var fieldName))
            {
                label = fieldName;
                return true;
            }

            label = $"offset 0x{offset:X} in {info.TypeName}";
            return true;
        }

        label = string.Empty;
        return false;
    }

    public void ApplyInstruction(Instruction instr)
    {
        switch (instr.Mnemonic)
        {
            case Mnemonic.Mov:
            case Mnemonic.Movzx:
            case Mnemonic.Movsx:
            case Mnemonic.Movsxd:
                ApplyMov(instr);
                break;
            case Mnemonic.Lea:
                ApplyLea(instr);
                break;
            case Mnemonic.Xchg:
                ApplyXchg(instr);
                break;
            case Mnemonic.Add:
                ApplyAddSub(instr, add: true);
                break;
            case Mnemonic.Sub:
                ApplyAddSub(instr, add: false);
                break;
            case Mnemonic.Xor:
            case Mnemonic.And:
                ApplyZeroing(instr);
                break;
            case Mnemonic.Pop:
                ApplyPop(instr);
                break;
            case Mnemonic.Call:
                Clear(Register.EAX);
                Clear(Register.EDX);
                break;
        }
    }

    private void ApplyMov(Instruction instr)
    {
        if (instr.Op0Kind != OpKind.Register)
            return;

        var dest = instr.Op0Register;

        switch (instr.Op1Kind)
        {
            case OpKind.Register:
                if (_registers.TryGetValue(instr.Op1Register, out var srcInfo))
                    _registers[dest] = srcInfo;
                else
                    Clear(dest);
                break;
            case OpKind.Immediate32:
            case OpKind.Immediate32to64:
                var imm = (uint)instr.GetImmediate(1);
                if (_singletonTypes.TryGetValue(imm, out var typeName))
                    SetSingletonAddress(dest, typeName);
                else
                    Clear(dest);
                break;
            case OpKind.Memory:
                if (IsAbsoluteMemoryOperand(instr) && _singletonTypes.TryGetValue((uint)instr.MemoryDisplacement32, out var absType))
                {
                    SetStruct(dest, absType, 0);
                    return;
                }

                if (_registers.TryGetValue(instr.MemoryBase, out var baseInfo) && baseInfo.Kind == RegisterValueKind.SingletonPointerAddress && instr.MemoryIndex == Register.None)
                {
                    SetStruct(dest, baseInfo.TypeName, 0);
                    return;
                }

                if (instr.MemoryIndex == Register.None && _registers.TryGetValue(instr.MemoryBase, out var structInfo) && structInfo.Kind == RegisterValueKind.StructBase)
                {
                    var offset = structInfo.BaseOffset + (int)instr.MemoryDisplacement32;
                    if (StructFieldRegistry.TryGetPointerTarget(structInfo.TypeName, offset, out var targetType))
                    {
                        SetStruct(dest, targetType, 0);
                        return;
                    }
                }

                Clear(dest);
                break;
            default:
                Clear(dest);
                break;
        }
    }

    private void ApplyLea(Instruction instr)
    {
        if (instr.Op0Kind != OpKind.Register || instr.Op1Kind != OpKind.Memory)
            return;

        var dest = instr.Op0Register;
        var baseReg = instr.MemoryBase;
        var disp = (int)instr.MemoryDisplacement32;

        if (_registers.TryGetValue(baseReg, out var baseInfo) && baseInfo.Kind == RegisterValueKind.StructBase && instr.MemoryIndex == Register.None)
        {
            SetStruct(dest, baseInfo.TypeName, baseInfo.BaseOffset + disp);
            return;
        }

        Clear(dest);
    }

    private void ApplyXchg(Instruction instr)
    {
        if (instr.Op0Kind != OpKind.Register || instr.Op1Kind != OpKind.Register)
            return;

        var r0 = instr.Op0Register;
        var r1 = instr.Op1Register;

        _registers.TryGetValue(r0, out var i0);
        _registers.TryGetValue(r1, out var i1);

        if (i1.Kind == RegisterValueKind.Unknown)
            Clear(r0);
        else
            _registers[r0] = i1;

        if (i0.Kind == RegisterValueKind.Unknown)
            Clear(r1);
        else
            _registers[r1] = i0;
    }

    private void ApplyAddSub(Instruction instr, bool add)
    {
        if (instr.Op0Kind != OpKind.Register)
            return;

        if (instr.Op1Kind is not (OpKind.Immediate8 or OpKind.Immediate32 or OpKind.Immediate32to64))
            return;

        var reg = instr.Op0Register;
        if (!_registers.TryGetValue(reg, out var info) || info.Kind != RegisterValueKind.StructBase)
            return;

        var imm = (int)instr.GetImmediate(1);
        var delta = add ? imm : -imm;
        SetStruct(reg, info.TypeName, info.BaseOffset + delta);
    }

    private void ApplyZeroing(Instruction instr)
    {
        if (instr.Op0Kind == OpKind.Register && instr.Op1Kind == OpKind.Register && instr.Op0Register == instr.Op1Register)
            Clear(instr.Op0Register);
    }

    private void ApplyPop(Instruction instr)
    {
        if (instr.Op0Kind == OpKind.Register)
            Clear(instr.Op0Register);
    }

    private void SetStruct(Register reg, string typeName, int baseOffset)
    {
        _registers[reg] = new RegisterTypeInfo(typeName, baseOffset, RegisterValueKind.StructBase);
    }

    private void SetSingletonAddress(Register reg, string typeName)
    {
        _registers[reg] = new RegisterTypeInfo(typeName, 0, RegisterValueKind.SingletonPointerAddress);
    }

    private void Clear(Register reg)
    {
        _registers.Remove(reg);
    }

    private static bool IsAbsoluteMemoryOperand(Instruction instr)
    {
        return instr.MemoryBase == Register.None && instr.MemoryIndex == Register.None;
    }

    private static string? TryExtractTypeName(string value)
    {
        var singletonStart = value.IndexOf('<');
        var singletonEnd = value.IndexOf('>');
        if (singletonStart >= 0 && singletonEnd > singletonStart)
            return value[(singletonStart + 1)..singletonEnd];

        var sep = value.IndexOf("::", StringComparison.Ordinal);
        if (sep > 0)
            return value[..sep];

        return null;
    }
}
