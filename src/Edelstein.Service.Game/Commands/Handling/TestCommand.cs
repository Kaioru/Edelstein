using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Interactions.Miniroom;
using Edelstein.Service.Game.Interactions.Miniroom.Types;

namespace Edelstein.Service.Game.Commands.Handling
{
    public class TestCommand : AbstractCommand<object>
    {
        public override string Name => "Test";
        public override string Description => "A test command.";
        
        public TestCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(FieldUser sender, object option)
        {
            var tradingRoom = new TradingRoom();
            var tradingDialog = new MiniroomDialog(sender, tradingRoom);

            await sender.Interact(tradingDialog);
        }
    }
}