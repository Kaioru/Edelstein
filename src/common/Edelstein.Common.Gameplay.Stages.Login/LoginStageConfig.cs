namespace Edelstein.Common.Gameplay.Stages.Login
{
    public record LoginStageConfig : IServerStageInfo
    {
        public string ID { get; init; }
        public string Host { get; init; }
        public short Port { get; init; }

        public bool AutoRegister { get; init; } = false;
        public byte[] Worlds { get; init; }
    }
}
