using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations
{
    public class FieldSpeaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => 0;

        private readonly IField _field;

        public FieldSpeaker(IConversationContext context, IField field) : base(context)
            => _field = field;
    }
}