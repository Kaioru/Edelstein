using Edelstein.Service.Game.Fields.Continent;

namespace Edelstein.Service.Game.Conversations
{
    public class ContinentSpeaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => 0;

        private readonly Continent _continent;

        public ContinentSpeaker(
            IConversationContext context,
            Continent continent
        ) : base(context)
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