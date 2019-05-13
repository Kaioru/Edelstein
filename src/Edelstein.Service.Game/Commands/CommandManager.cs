using System.Collections.Generic;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands
{
    public class CommandManager : AbstractCommand<object>
    {
        public const char Prefix = '!';

        public override string Name => "manager";
        public override string Description => "The root command";

        public CommandManager(Parser parser) : base(parser)
        {
        }

        protected override Task Execute(FieldUser sender, object option)
        {
            return Process(sender, new Queue<string>(new[] {"--help"}));
        }
    }
}