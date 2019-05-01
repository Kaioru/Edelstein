using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Conversations.Speakers
{
    public class FieldNPCSpeaker : AbstractFieldObjSpeaker<FieldNPC>
    {
        public override int TemplateID => Obj.Template.ID;

        public FieldNPCSpeaker(IConversationContext context, FieldNPC obj) : base(context, obj)
        {
        }
    }
}