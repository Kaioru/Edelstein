using Edelstein.Service.Game.Fields.Continents;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields.Continents
{
    public class ContinentSpeaker : Speaker
    {
        private readonly Continent _continent;

        public ContinentSpeaker(IConversationContext context, Continent continent) : base(context)
        {
            _continent = continent;
        }

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