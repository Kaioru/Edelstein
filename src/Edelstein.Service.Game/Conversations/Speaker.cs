namespace Edelstein.Service.Game.Conversations
{
    public class Speaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID { get; }
        public override ScriptMessageParam Param { get; }

        public Speaker(
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