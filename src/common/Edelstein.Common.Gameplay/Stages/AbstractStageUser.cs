using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
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
        private IPacketProcessor<TStage, TUser> _processor;

        public virtual int ID => Character.ID;

        public Account Account { get; set; }
        public AccountWorld AccountWorld { get; set; }
        public Character Character { get; set; }

        public TStage Stage { get; set; }
        public ISocket Socket { get; init; }

        public AbstractStageUser(ISocket socket, IPacketProcessor<TStage, TUser> processor)
        {
            Socket = socket;
            _processor = processor;
        }

        public virtual Task OnPacket(IPacketReader packet) => _processor.Process((TUser)this, packet);
        public virtual Task OnException(Exception exception) => Disconnect();

        public virtual async Task OnDisconnect()
        {
            if (Stage != null)
                await Stage.Leave((TUser)this);
        }

        public abstract Task Update();
        public Task Disconnect() => Socket.Disconnect();
        public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);
        public Task Dispatch(IEnumerable<IPacket> packets) => Socket.Dispatch(packets);
    }
}
