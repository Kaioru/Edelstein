using System;
using System.Threading.Tasks;

namespace Edelstein.Core.Utils.Packets
{
    public class ActionPacketHandler : IPacketHandler
    {
        private readonly Func<IPacketHandlerContext, Task> _action;

        public ActionPacketHandler(Action<IPacketHandlerContext> action)
            => _action = ctx =>
            {
                action.Invoke(ctx);
                return Task.CompletedTask;
            };

        public ActionPacketHandler(Func<IPacketHandlerContext, Task> action)
            => _action = action;

        public Task Handle(IPacketHandlerContext ctx)
            => _action.Invoke(ctx);
    }
}