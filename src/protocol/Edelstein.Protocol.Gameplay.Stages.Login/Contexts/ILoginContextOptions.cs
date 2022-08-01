using Edelstein.Protocol.Gameplay.Stages.Contexts;

namespace Edelstein.Protocol.Gameplay.Stages.Login.Contexts;

public interface ILoginContextOptions : IStageContextOptions
{
    byte[] Worlds { get; }
}
