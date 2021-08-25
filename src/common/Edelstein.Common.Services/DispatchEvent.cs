using System.Collections.Generic;

namespace Edelstein.Common.Services
{
    public record DispatchEvent(
        byte[] Data,
        ICollection<string> TargetServers,
        ICollection<int> TargetCharacters
    );
}
