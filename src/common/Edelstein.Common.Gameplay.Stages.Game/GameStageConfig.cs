namespace Edelstein.Common.Gameplay.Stages.Game
{
    public record GameStageConfig : IServerStageInfo
    {
        public string ID { get; init; }
        public string Host { get; init; }
        public short Port { get; init; }

        public int WorldID { get; init; }
        public int ChannelID { get; init; }
    }
}
