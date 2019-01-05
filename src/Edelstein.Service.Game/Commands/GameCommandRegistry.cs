using CommandLine;
using Edelstein.Core.Commands;
using Edelstein.Service.Game.Commands.Handling;

namespace Edelstein.Service.Game.Commands
{
    public class GameCommandRegistry : CommandRegistry
    {
        public GameCommandRegistry(Parser parser) : base(parser)
        {
            Commands.Add(new TestCommand(parser));

            Commands.Add(new FieldCommand(parser));
            Commands.Add(new ItemCommand(parser));
        }
    }
}