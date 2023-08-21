using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStageLogin : ProgramConfigStage, ILoginStageOptions
{
    public byte[] Worlds { get; set; }
}
