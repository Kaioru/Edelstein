namespace Edelstein.Common.Gameplay.Stages.Game
{
    public record GameStageConfig : ServerStageConfig
    {
        public int WorldID { get; init; }
        public int ChannelID { get; init; }
    }
}
