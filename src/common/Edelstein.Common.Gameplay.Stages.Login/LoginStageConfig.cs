namespace Edelstein.Common.Gameplay.Stages.Login
{
    public record LoginStageConfig : ServerStageConfig
    {
        public bool AutoRegister { get; init; } = false;
    }
}
