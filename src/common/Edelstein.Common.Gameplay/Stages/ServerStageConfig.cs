namespace Edelstein.Common.Gameplay.Stages
{
    public record ServerStageConfig
    {
        public string ID { get; init; }

        public string ServerHost { get; init; }
        public short ServerPort { get; init; }

        public string InteropHost { get; init; }
        public short InteropPort { get; init; }
    }
}
