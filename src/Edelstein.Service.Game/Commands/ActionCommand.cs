using System;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands
{
    public class ActionCommand : AbstractCommand<object>
    {
        public override string Name { get; }
        public override string Description { get; }

        private readonly Action<FieldUser> _action;

        public ActionCommand(Parser parser, string name, string description, Action<FieldUser> action) : base(parser)
        {
            Name = name;
            Description = description;
            _action = action;
        }

        protected override Task Execute(FieldUser sender, object option)
        {
            _action.Invoke(sender);
            return Task.CompletedTask;
        }
    }
}