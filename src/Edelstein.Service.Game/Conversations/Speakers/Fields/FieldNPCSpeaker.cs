using Edelstein.Service.Game.Fields.Objects.NPC;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public class FieldNPCSpeaker : FieldObjSpeaker<FieldNPC>
    {
        public override int TemplateID => Obj.Template.ID;

        public FieldNPCSpeaker(IConversationContext context, FieldNPC obj) : base(context, obj)
        {
        }
        
        public ISpeaker AsSpeaker(SpeakerParamType param = 0)
            => new Speaker(Context, TemplateID, param);
    }
}