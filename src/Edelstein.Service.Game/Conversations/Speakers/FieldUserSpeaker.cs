using MoonSharp.Interpreter;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    [MoonSharpUserData]
    public class FieldUserSpeaker : AbstractSpeaker
    {
        public override int TemplateID => 9010000;
        public override SpeakerParamType ParamType => 0;

        public FieldUserSpeaker(IConversationContext context) : base(context)
        {
        }
    }
}