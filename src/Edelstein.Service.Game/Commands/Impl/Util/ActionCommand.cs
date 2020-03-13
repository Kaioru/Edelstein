using System;
using System.Threading.Tasks;
using CommandLine;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Commands.Impl.Util
{
    public class ActionCommand : AbstractCommand<DefaultCommandContext>
    {
        public override string Name { get; }
        public override string Description { get; }

        private readonly Action<FieldUser, DefaultCommandContext> _action;

        public ActionCommand(Parser parser, string name, string description, Action<FieldUser, DefaultCommandContext> action) : base(parser)
        {
            Name = name;
            Description = description;
            _action = action;
        }

        protected override Task Run(FieldUser sender, DefaultCommandContext ctx)
        {
            _action.Invoke(sender, ctx);
            return Task.CompletedTask;
        }
    }
}