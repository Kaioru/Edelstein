using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using MoreLinq;

namespace Edelstein.Common.Gameplay.Handling
{
    public class PacketProcessor<TStage, TUser> : IPacketProcessor<TStage, TUser>
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        private readonly ILogger<PacketProcessor<TStage, TUser>> _logger;
        private readonly IDictionary<short, IPacketHandler<TStage, TUser>> _handlers;

        public PacketProcessor(
            IEnumerable<IPacketHandler<TStage, TUser>> handlers,
            ILogger<PacketProcessor<TStage, TUser>> logger = null
        )
        {
            _handlers = handlers
                .DistinctBy(h => h.Operation)
                .ToDictionary(
                    h => h.Operation,
                    h => h
                );
            _logger = logger ?? new NullLogger<PacketProcessor<TStage, TUser>>();
        }

        public void Register(IPacketHandler<TStage, TUser> handler)
        {
            if (!_handlers.ContainsKey(handler.Operation))
                _handlers[handler.Operation] = handler;
        }

        public void Deregister(IPacketHandler<TStage, TUser> handler)
        {
            _handlers.Remove(handler.Operation);
        }

        public async Task Process(TUser user, IPacketReader packet)
        {
            var operation = packet.ReadShort();

            if (!_handlers.ContainsKey(operation))
            {
                _logger.LogWarning($"Unhandled {typeof(TStage).Name} packet operation 0x{operation:X} ({Enum.GetName((PacketRecvOperations)operation)})");
                return;
            }

            var handler = _handlers[operation];

            if (await handler.Check(user))
                await handler.Handle(user, packet);
        }
    }
}
