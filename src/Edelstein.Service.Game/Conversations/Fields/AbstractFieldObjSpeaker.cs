using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations.Fields
{
    public abstract class AbstractFieldObjSpeaker<T> : AbstractSpeaker
        where T : IFieldObj
    {
        protected T Obj { get; }

        protected AbstractFieldObjSpeaker(IConversationContext context, T obj) : base(context)
            => Obj = obj;

        public ISpeaker AsField() => new FieldSpeaker(Context, Obj.Field);
        public ISpeaker AsContinent() => AsContinent(Obj.Field.ID);
    }
}