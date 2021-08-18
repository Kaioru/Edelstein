using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Common
{
    public class DebugCommandArgs : CommandArgs
    {
        [ArgRequired, ArgPosition(0), ArgRange(0, 50)]
        public int Number { get; set; }
    }

    public class DebugCommand : AbstractCommand<DebugCommandArgs>
    {
        public override string Name => "debug";
        public override string Description => "A testing debug command";

        public override async Task Execute(IFieldObjUser user, DebugCommandArgs args)
        {
            await user.Message($"Input number: {args.Number}");
        }
    }
}
