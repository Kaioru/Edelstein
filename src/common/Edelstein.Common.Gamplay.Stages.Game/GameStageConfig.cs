namespace Edelstein.Common.Gameplay.Stages.Game
{
    public record GameStageConfig : MigrateableStageConfig
    {
        public int WorldID { get; init; }
        public int ChannelID { get; init; }
    }
}
