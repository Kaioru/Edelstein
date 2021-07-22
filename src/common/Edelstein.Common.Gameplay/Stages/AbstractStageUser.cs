using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractStageUser<TStage, TUser> : IStageUser<TStage, TUser>
        where TStage : AbstractStage<TStage, TUser>
        where TUser : AbstractStageUser<TStage, TUser>
    {
        public Account Account { get; set; }
        public AccountWorld AccountWorld { get; set; }
        public Character Character { get; set; }

        public TStage Stage { get; set; }

        public string SessionID => Socket.ID;
        public ISocket Socket { get; init; }

        public AbstractStageUser(ISocket socket)
            => Socket = socket;

        public Task OnPacket(IPacketReader packet) => Stage.Processor.Process((TUser)this, packet);
        public abstract Task OnException(Exception exception);
        public abstract Task OnDisconnect();

        public Task Disconnect() => Socket.Disconnect();
        public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);
        public Task Dispatch(IEnumerable<IPacket> packets) => Socket.Dispatch(packets);
    }
}
