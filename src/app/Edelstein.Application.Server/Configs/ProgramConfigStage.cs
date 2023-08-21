using Edelstein.Protocol.Utilities.Repositories;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStage : IIdentifiable<string>
{
    public string ID { get; set; }
    
    public string Host { get; set; }
    public short Port { get; set; }

    public short Version { get; set; } = 95;
    public string Patch { get; set; } = "1";
    public byte Locale { get; set; } = 8;
}
