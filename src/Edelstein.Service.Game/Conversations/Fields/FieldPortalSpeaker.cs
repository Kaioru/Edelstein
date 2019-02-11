using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations.Fields
{
    public class FieldPortalSpeaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => 0;

        private readonly FieldPortalTemplate portal;
        private readonly IField field;

        public FieldPortalSpeaker(
            IConversationContext context,
            FieldPortalTemplate portal,
            IField field
        ) : base(context)
        {
            this.portal = portal;
            this.field = field;
        }

        public ISpeaker AsField() => new FieldSpeaker(Context, field);
    }
}