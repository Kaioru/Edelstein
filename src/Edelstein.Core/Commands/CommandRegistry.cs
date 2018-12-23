using System.Collections.Generic;
using System.Threading.Tasks;
using CommandLine;

namespace Edelstein.Core.Commands
{
    public class CommandRegistry : AbstractCommand<object>
    {
        public override string Name { get; }
        public override string Description { get; }

        public CommandRegistry(Parser parser) : base(parser)
        {
        }

        protected override Task Execute(ICommandSender sender, object option)
            => Process(sender, new Queue<string>(new[] {"--help"}));
    }
}