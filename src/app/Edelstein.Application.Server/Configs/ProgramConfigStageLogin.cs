using Edelstein.Protocol.Gameplay.Login;

namespace Edelstein.Application.Server.Configs;

public record ProgramConfigStageLogin : ProgramConfigStage, ILoginStageOptions
{
    public byte[] Worlds { get; set; }

    public bool IsAutoRegister { get; set; } = true;
    public bool IsFlippedUsername { get; set; } = false;
}
