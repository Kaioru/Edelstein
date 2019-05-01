using System;
using System.Collections.Generic;
using Edelstein.Service.Game.Conversations.Speakers.Fields;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public static class Speakers
    {
        public static readonly ICollection<Type> Types = new List<Type>
        {
            typeof(FieldSpeaker),
            typeof(FieldPortalSpeaker),
            typeof(FieldUserSpeaker),
            typeof(FieldNPCSpeaker)
        };
    }
}