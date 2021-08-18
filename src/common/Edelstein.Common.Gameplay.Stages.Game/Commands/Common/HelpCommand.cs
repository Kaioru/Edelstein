using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Common
{
    public class HelpCommand : AbstractCommand
    {
        public override string Name => "help";
        public override string Description => "Shows all available commands";

        private readonly ICommandProcessor _processor;

        public HelpCommand(ICommandProcessor processor)
        {
            _processor = processor;

            Aliases.Add("commands");
            Aliases.Add("?");
        }

        public override async Task Execute(IFieldObjUser user, string[] args)
        {
            await user.Message("Available commands:");
            await Task.WhenAll(_processor.Commands.Select(c => user.Message($"{c.Name} - {c.Description}")));
        }
    }
}
