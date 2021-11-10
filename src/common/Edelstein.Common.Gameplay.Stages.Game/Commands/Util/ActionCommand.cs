using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Common.Gameplay.Stages.Game.Commands.Util
{
    public class ActionCommand : AbstractCommand
    {
        public override string Name { get; }
        public override string Description { get; }

        private readonly Func<IFieldObjUser, string[], Task> _func;

        public ActionCommand(string name, string description, Func<IFieldObjUser, string[], Task> func)
        {
            Name = name;
            Description = description;
            _func = func;
        }

        public override Task Execute(IFieldObjUser user, string[] args)
            => _func.Invoke(user, args);
    }
}
