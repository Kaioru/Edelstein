using System.Threading.Tasks;
using Edelstein.Core.Bootstrap;
using Edelstein.Core.Distributed.Peers.Info;
using Edelstein.Provider.Templates;
using Edelstein.Service.Trade.Services;

namespace Edelstein.Service.Trade
{
    internal static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .WithTemplates(TemplateCollectionType.None)
                .Start<TradeService, TradeServiceInfo>(args);
    }
}