using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game;
using Edelstein.Protocol.Network.Transport;
using Microsoft.Extensions.Hosting;

namespace Edelstein.App.Game
{
    public class ProgramHost : IHostedService
    {
        private readonly GameStage _stage;
        private readonly ITransportAcceptor _acceptor;

        public ProgramHost(GameStage stage, ITransportAcceptor acceptor)
        {
            _stage = stage;
            _acceptor = acceptor;
        }

        public Task StartAsync(CancellationToken cancellationToken)
            => _acceptor.Accept(_stage.Info.Host, _stage.Info.Port);

        public Task StopAsync(CancellationToken cancellationToken)
            => _acceptor.Close();
    }
}
