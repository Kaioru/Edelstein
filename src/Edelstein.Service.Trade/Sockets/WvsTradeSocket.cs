using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Service.Trade.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Trade.Sockets
{
    public partial class WvsTradeSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public WvsTrade WvsTrade { get; set; }
        public Character Character { get; set; }

        public WvsTradeSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsTrade wvsTrade
        ) : base(channel, seqSend, seqRecv)
            => WvsTrade = wvsTrade;

        public override async Task OnDisconnect()
        {
            if (Character == null) return;
            if (!ReadOnlyMode) await OnUpdate();

            var account = Character.Data.Account;
            var state = await WvsTrade.AccountStatusCache.GetAsync<AccountState>(account.ID.ToString());

            if (state.HasValue && state.Value != AccountState.MigratingIn)
                await WvsTrade.AccountStatusCache.RemoveAsync(account.ID.ToString());
        }

        public override async Task OnUpdate()
        {
            if (Character == null) return;
            
            using (var db = WvsTrade.DataContextFactory.Create())
            {
                db.Update(Character);
                db.SaveChanges();
            }
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Caught exception in socket handling");
            return Task.CompletedTask;
        }
    }
}