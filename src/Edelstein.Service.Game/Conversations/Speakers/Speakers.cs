using System;
using System.Collections.Generic;
using Edelstein.Service.Game.Conversations.Speakers.Fields;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Continents;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Inventories;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public static class Speakers
    {
        public static readonly ICollection<Type> Types = new List<Type>
        {
            typeof(Speaker),
            typeof(FieldSpeaker),
            typeof(ContinentSpeaker),
            typeof(FieldPortalSpeaker),
            typeof(FieldUserSpeaker),
            typeof(FieldUserInventorySpeaker),
            typeof(FieldNPCSpeaker)
        };
    }
}