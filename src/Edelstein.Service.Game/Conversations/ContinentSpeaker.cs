using Edelstein.Service.Game.Fields.Continent;

namespace Edelstein.Service.Game.Conversations
{
    public class ContinentSpeaker : Speaker
    {
        private Continent _continent;

        public ContinentSpeaker(
            IConversationContext context,
            Continent continent,
            int templateID = 9010000,
            ScriptMessageParam param = (ScriptMessageParam) 0
        ) : base(context, templateID, param)
            => _continent = continent;

        public int StartShipMoveField => _continent.Template.StartShipMoveFieldID;
        public int WaitField => _continent.Template.WaitFieldID;
        public int MoveField => _continent.Template.MoveFieldID;
        public int EndField => _continent.Template.EndFieldID;
        public int EndShipMoveField => _continent.Template.EndShipMoveFieldID;

        public ContinentState State => _continent.State;

        public int Term => _continent.Template.Term;
        public int Delay => _continent.Template.Delay;
        public int Wait => _continent.Template.Wait;
        public int Required => _continent.Template.Required;
    }
}