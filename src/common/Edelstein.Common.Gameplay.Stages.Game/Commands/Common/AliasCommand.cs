using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Common
{
    public class AliasCommand : AbstractCommand
    {
        public override string Name => "alias";
        public override string Description => "Shows all available command aliases";

        private readonly ICommandProcessor _processor;

        public AliasCommand(ICommandProcessor processor)
        {
            _processor = processor;

            Aliases.Add("aliases");
        }

        public override async Task Execute(IFieldObjUser user, string[] args)
        {
            await user.Message("Available command aliases:");
            await Task.WhenAll(_processor.Commands
                .Where(c => c.Aliases.Count > 0)
                .Select(c => user.Message($"{c.Name} - {string.Join(", ", c.Aliases)}")));
        }
    }
}
