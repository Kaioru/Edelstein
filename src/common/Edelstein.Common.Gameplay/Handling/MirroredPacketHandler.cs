using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Gameplay.Handling
{
    public class MirroredPacketHandler<TStage, TUser> : AbstractPacketHandler<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        private readonly short _operation;
        private readonly IPacketHandler<TStage, TUser> _handler;

        public override short Operation => _operation;

        public MirroredPacketHandler(short operation, IPacketHandler<TStage, TUser> handler)
        {
            _operation = operation;
            _handler = handler;
        }

        public override Task<bool> Check(TUser user) => _handler.Check(user);
        public override Task Handle(TUser user, IPacketReader packet) => _handler.Handle(user, packet);
    }
}
