using Edelstein.Service.Game.Conversations.Speakers.Fields.Continents;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public abstract class FieldObjSpeaker<T> : Speaker
        where T : IFieldObj
    {
        public T Obj { get; }

        protected FieldObjSpeaker(IConversationContext context, T obj) : base(context)
            => Obj = obj;

        public FieldSpeaker GetField() => new FieldSpeaker(Context, Obj.Field);

        public ContinentSpeaker GetContinent()
            => GetField().GetContinent();
    }
}