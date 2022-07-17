using Edelstein.Protocol.Util.Repositories;

#pragma warning disable CS8618

namespace Edelstein.Daemon.Server.Configs;

public abstract record AbstractProgramConfigStage : IIdentifiable<string>
{
    public string Host { get; set; }
    public short Port { get; set; }

    public short Version { get; set; } = 95;
    public string Patch { get; set; } = "1";
    public byte Locale { get; set; } = 8;

    public string ID { get; set; }
}
