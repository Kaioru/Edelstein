using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public class FieldPortalSpeaker : Speaker
    {
        private readonly IFieldPortal _portal;

        public FieldPortalSpeaker(IConversationContext context, IFieldPortal portal) : base(context)
            => _portal = portal;

        public void Enter(FieldUserSpeaker speaker)
            => _portal.Enter(speaker.Obj).Wait();
    }
}