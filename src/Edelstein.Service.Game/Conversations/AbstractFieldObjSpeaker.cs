using System.Linq;
using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations
{
    public abstract class AbstractFieldObjSpeaker<T> : AbstractSpeaker
        where T : IFieldObj
    {
        protected T Obj { get; }

        protected AbstractFieldObjSpeaker(IConversationContext context, T obj) : base(context)
            => Obj = obj;

        public ISpeaker AsField() => new FieldSpeaker(Context, Obj.Field);

        public ISpeaker AsContinent()
        {
            var continent = Context.Socket.WvsGame.ContinentManager.Continents.FirstOrDefault(c =>
                c.Template.StartShipMoveFieldID == Obj.Field.ID ||
                c.Template.WaitFieldID == Obj.Field.ID ||
                c.Template.MoveFieldID == Obj.Field.ID);

            return continent == null
                ? null
                : new ContinentSpeaker(Context, continent);
        }
    }
}