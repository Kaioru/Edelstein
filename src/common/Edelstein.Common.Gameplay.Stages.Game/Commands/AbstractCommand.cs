using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Commands;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using PowerArgs;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands
{
    public abstract class AbstractCommand<TArgs> : AbstractCommand where TArgs : CommandArgs
    {
        public override async Task Execute(IFieldObjUser user, string[] args)
        {
            var def = new CommandLineArgumentsDefinition(typeof(TArgs)) { ExeName = Name };

            try
            {
                var parsed = Args.Parse<TArgs>(args);

                await Execute(user, parsed);
            }
            catch (ArgException e)
            {
                await user.Message(e.Message);
                await user.Message($"Usage: {def.UsageSummary}");
            }
        }

        public abstract Task Execute(IFieldObjUser user, TArgs args);
    }

    public abstract class AbstractCommand : CommandProcessor, ICommand
    {
        public abstract string Name { get; }
        public abstract string Description { get; }

        public ICollection<string> Aliases { get; }

        public AbstractCommand()
            => Aliases = new List<string>();

        public override async Task<bool> Process(IFieldObjUser user, string[] args)
        {
            if (!await base.Process(user, args))
            {
                await Execute(user, args);
                return true;
            }

            return false;
        }

        public abstract Task Execute(IFieldObjUser user, string[] args);
    }
}
