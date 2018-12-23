using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Conversations
{
    public class FieldUserSpeaker : AbstractFieldObjSpeaker<FieldUser>
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => ScriptMessageParam.NPCReplacedByUser ;

        public FieldUserSpeaker(IConversationContext context, FieldUser obj) : base(context, obj)
        {
        }
    }
}