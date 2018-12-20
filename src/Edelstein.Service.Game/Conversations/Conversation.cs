using System;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversations
{
    public class Conversation : AbstractConversation
    {
        private readonly Action<ISpeaker, ISpeaker> _action;

        public Conversation(
            IConversationContext context,
            ISpeaker self,
            ISpeaker target,
            Action<ISpeaker, ISpeaker> action
        ) : base(context, self, target)
            => _action = action;

        public override Task Start()
        {
            _action.Invoke(Self, Target);
            return Task.CompletedTask;
        }
    }
}