namespace Edelstein.Common.Gameplay.Stages
{
    public record ServerStageConfig
    {
        public string ID { get; init; }

        public string Host { get; init; }
        public short Port { get; init; }
    }
}
