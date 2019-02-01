using System;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Service.Shop.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Shop.Sockets
{
    public partial class WvsShopSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public WvsShop WvsShop { get; set; }
        public Character Character { get; set; }

        public WvsShopSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsShop wvsShop
        ) : base(channel, seqSend, seqRecv)
            => WvsShop = wvsShop;

        public override async Task OnDisconnect()
        {
            if (Character == null) return;

            try
            {
                using (var db = WvsShop.DataContextFactory.Create())
                {
                    var account = Character.Data.Account;
                    var state = await WvsShop.AccountStatusCache.GetAsync<AccountState>(account.ID.ToString());

                    if (state.HasValue && state.Value != AccountState.MigratingIn)
                        await WvsShop.AccountStatusCache.RemoveAsync(account.ID.ToString());

                    db.Update(Character);
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public override Task OnException(Exception exception)
        {
            Logger.Error(exception, "Caught exception in socket handling");
            return Task.CompletedTask;
        }
    }
}