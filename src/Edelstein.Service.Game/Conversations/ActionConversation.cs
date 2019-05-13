using System;
using System.Threading.Tasks;
using Edelstein.Service.Game.Conversations.Speakers;

namespace Edelstein.Service.Game.Conversations
{
    public class ActionConversation : IConversation
    {
        private readonly Action<ISpeaker, ISpeaker> _action;

        public IConversationContext Context { get; }
        public ISpeaker Self { get; }
        public ISpeaker Target { get; }

        public ActionConversation(IConversationContext context, ISpeaker self, ISpeaker target,
            Action<ISpeaker, ISpeaker> action)
        {
            Context = context;
            Self = self;
            Target = target;
            _action = action;
        }

        public Task Start()
        {
            _action.Invoke(Self, Target);
            return Task.CompletedTask;
        }
    }
}