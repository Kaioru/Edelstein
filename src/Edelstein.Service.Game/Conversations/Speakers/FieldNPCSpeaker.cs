namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class FieldNPCSpeaker : AbstractSpeaker
    {
        public override int TemplateID => 9010000;
        public override SpeakerParamType ParamType => SpeakerParamType.NPCReplacedByUser;

        public FieldNPCSpeaker(IConversationContext context) : base(context)
        {
        }
    }
}