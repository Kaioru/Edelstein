using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Interactions.Miniroom.Trade;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class TestCommand : AbstractGameCommand<TestCommandOption>
    {
        public override string Name => "Test";
        public override string Description => "Testing!";

        private readonly TradingRoom _room = new TradingRoom();
        
        public TestCommand(Parser parser) : base(parser)
        {
        }

        protected override Task ExecuteAfter(FieldUser user, TestCommandOption option)
        {
            return user.Interact(_room);
        }
    }

    public class TestCommandOption
    {
    }
}