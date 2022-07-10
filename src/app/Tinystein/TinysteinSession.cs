using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Session;
using Edelstein.Protocol.Network.Transport;
using Tinystein.Logging;

namespace Tinystein
{
    public class TinysteinSession : ISession
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public string SessionID => Socket.ID;
        public ISocket Socket { get; init; }

        public TinysteinSession(ISocket socket)
            => Socket = socket;

        public Task Dispatch(IPacket packet) => Socket.Dispatch(packet);
        public Task Update() => Task.CompletedTask;
        public Task Disconnect() => Socket.Disconnect();

        public Task OnDisconnect()
        {
            Logger.Info("Socket disconnected");
            return Task.CompletedTask;
        }

        public Task OnException(Exception exception)
        {
            Logger.WarnException("Socket caught exception", exception);
            return Task.CompletedTask;
        }

        public Task OnPacket(IPacketReader packet)
        {
            var operation = packet.ReadShort();

            Logger.Info($"Received packet of operation 0x{operation:X} ({operation}) of length {packet.Buffer.Length}");
            Logger.Debug($"Received packet payload: {string.Join(" ", packet.Buffer.ToArray().Select(b => "0x" + b.ToString("X")))}");
            return Task.CompletedTask;
        }
    }
}
