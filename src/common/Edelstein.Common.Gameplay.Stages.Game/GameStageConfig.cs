using System.Collections.Generic;

namespace Edelstein.Common.Gameplay.Stages.Game
{
    public record GameStageConfig : ServerStageInfo
    {
        public int WorldID { get; init; }
        public int ChannelID { get; init; }

        public override void AddMetadata(IDictionary<string, string> metadata)
        {
            base.AddMetadata(metadata);

            metadata["WorldID"] = WorldID.ToString();
            metadata["ChannelID"] = ChannelID.ToString();
        }
    }
}
