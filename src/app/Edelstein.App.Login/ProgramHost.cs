using System.Threading;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Login;
using Edelstein.Protocol.Network.Transport;
using Microsoft.Extensions.Hosting;

namespace Edelstein.App.Login
{
    public class ProgramHost : IHostedService
    {
        private readonly LoginStage _stage;
        private readonly ITransportAcceptor _acceptor;

        public ProgramHost(LoginStage stage, ITransportAcceptor acceptor)
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
