namespace Edelstein.Service.Game.Conversation.Speakers
{
    public class DefaultSpeaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID { get; }
        public override ScriptMessageParam Param { get; }

        public DefaultSpeaker(
            IConversationContext context,
            int templateID = 9010000,
            ScriptMessageParam param = 0
        ) : base(context)
        {
            TemplateID = templateID;
            Param = param;
        }
    }
}