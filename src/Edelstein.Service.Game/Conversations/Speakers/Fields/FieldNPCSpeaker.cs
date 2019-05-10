using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.NPCs;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public class FieldNPCSpeaker : FieldObjSpeaker<FieldNPC>
    {
        public override int TemplateID => Obj.Template.ID;

        public FieldNPCSpeaker(IConversationContext context, FieldNPC obj) : base(context, obj)
        {
        }
    }
}