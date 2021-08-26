using System.Collections.Generic;

namespace Edelstein.Common.Services.Social
{
    public record PartyUpdateEvent
    {
        public PartyRecord Party { get; init; }

        public ICollection<int> JoiningCharacters { get; init; }
        public ICollection<int> LeavingCharacters { get; init; }

        public PartyUpdateEvent()
        {
            JoiningCharacters = new List<int>();
            LeavingCharacters = new List<int>();
        }
    }
}
