﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Conversations;
using Edelstein.Protocol.Scripting;

namespace Edelstein.Common.Gameplay.Stages.Game.Conversations.Scripted
{
    public class ScriptedConversation : AbstractConversation
    {
        private readonly string _name;
        private readonly IScript _script;

        public ScriptedConversation(
            IConversationContext context,
            IConversationSpeaker self,
            IConversationSpeaker target,
            string name,
            IScript script
        ) : base(context, self, target)
        {
            _name = name;
            _script = script;
        }

        public override async Task Start()
        {
            await _script.Run(new Dictionary<string, object>
            {
                ["self"] = Self,
                ["target"] = Target
            });
        }
    }
}
