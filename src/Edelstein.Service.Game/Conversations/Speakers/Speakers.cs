using System;
using System.Collections.Generic;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class Speakers
    {
        public static ICollection<Type> Types = new List<Type>
        {
            typeof(FieldUserSpeaker),
            typeof(FieldNPCSpeaker)
        };
    }
}