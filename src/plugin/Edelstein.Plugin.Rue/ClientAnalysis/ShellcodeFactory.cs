using Iced.Intel;
using static Iced.Intel.AssemblerRegisters;

namespace Edelstein.Plugin.Rue.ClientAnalysis;

public sealed class ShellcodeFactory
{
    public byte[] BuildSendLoginPacketShellcode(IntPtr shellcodeAddr, IntPtr cLoginThis, IntPtr funcAddr, int worldId, int channelId)
    {
        if (shellcodeAddr == IntPtr.Zero) throw new ArgumentException("Shellcode address is required.", nameof(shellcodeAddr));
        if (cLoginThis == IntPtr.Zero) throw new ArgumentException("CLogin instance address is required.", nameof(cLoginThis));
        if (funcAddr == IntPtr.Zero) throw new ArgumentException("Function address is required.", nameof(funcAddr));

        var asm = new Assembler(32);

        asm.push(channelId);
        asm.push(worldId);
        asm.mov(ecx, cLoginThis.ToInt32());
        asm.call((ulong)funcAddr.ToInt32());
        asm.ret();

        using var stream = new MemoryStream();
        var writer = new StreamCodeWriter(stream);
        asm.Assemble(writer, (ulong)shellcodeAddr.ToInt64());

        return stream.ToArray();
    }
}
