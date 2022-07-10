using System.Collections.Generic;

namespace Edelstein.Common.Gameplay.Stages
{
    public record ServerStageInfo
    {
        public string ID { get; init; }

        public string Host { get; init; }
        public short Port { get; init; }

        public virtual void AddMetadata(IDictionary<string, string> metadata)
        {
            metadata["ID"] = ID;
            metadata["Host"] = Host;
            metadata["Port"] = Port.ToString();
        }
    }
}
