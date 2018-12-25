using System.Threading.Tasks;
using CommandLine;
using Edelstein.Core.Commands;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Commands
{
    public abstract class AbstractGameCommand<T> : AbstractCommand<T>
    {
        public AbstractGameCommand(Parser parser) : base(parser)
        {
        }

        protected override async Task Execute(ICommandSender sender, T option)
        {
            if (sender is FieldUser user)
                await ExecuteAfter(user, option);
        }

        protected abstract Task ExecuteAfter(FieldUser user, T option);
    }
}