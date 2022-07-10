namespace Edelstein.Common.Gameplay.Stages.Login
{
    public record LoginStageConfig : ServerStageInfo
    {
        public bool AutoRegister { get; init; } = false;
        public byte[] Worlds { get; init; }
    }
}
