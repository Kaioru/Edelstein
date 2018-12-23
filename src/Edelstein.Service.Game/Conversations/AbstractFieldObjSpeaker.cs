using Edelstein.Service.Game.Field;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractFieldObjSpeaker<T> : AbstractSpeaker
        where T : IFieldObj
    {
        protected T Obj { get; }

        protected AbstractFieldObjSpeaker(IConversationContext context, T obj) : base(context)
            => Obj = obj;

        public ISpeaker AsField() => new FieldSpeaker(Context, Obj.Field);
    }
}