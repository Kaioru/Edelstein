using System;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Conversation.Impl
{
    public class FuncConversation : AbstractConversation
    {
        private readonly Func<ISpeaker, ISpeaker, Task> _func;

        public FuncConversation(
            IConversationContext context,
            ISpeaker self,
            ISpeaker target,
            Func<ISpeaker, ISpeaker, Task> func
        ) : base(context, self, target)
            => _func = func;

        public override Task Start()
            => _func.Invoke(Self, Target);
    }
}