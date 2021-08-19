using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Common
{
    public class DebugCommandArgs : CommandArgs
    {
    }

    public class DebugCommand : AbstractCommand<DebugCommandArgs>
    {
        public override string Name => "Debug";
        public override string Description => "A testing debug command";

        public override async Task Execute(IFieldObjUser user, DebugCommandArgs args)
        {
            var number = await user.Prompt(target => target.AskNumber("What number?"));

            await user.Message($"Input number: {number}");
        }
    }
}
