using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Conversations.Speakers.Field
{
    public class FieldObjSpeaker<T> : DefaultSpeaker
        where T : IFieldObj
    {
        protected T Obj { get; }

        protected FieldObjSpeaker(IConversationContext context, T obj) : base(context)
        {
            Obj = obj;
        }
    }
}