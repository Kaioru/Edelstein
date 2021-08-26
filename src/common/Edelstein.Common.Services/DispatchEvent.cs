using System.Collections.Generic;

namespace Edelstein.Common.Services
{
    public record DispatchEvent
    {
        public byte[] Data { get; init; }
        public ICollection<string> TargetServers { get; init; }
        public ICollection<int> TargetCharacters { get; init; }

        public DispatchEvent()
        {
            TargetServers = new List<string>();
            TargetCharacters = new List<int>();
        }
    }
}
