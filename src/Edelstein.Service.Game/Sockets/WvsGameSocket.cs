using System;
using System.Linq;
using System.Threading.Tasks;
using DotNetty.Transport.Channels;
using Edelstein.Data.Entities;
using Edelstein.Network;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Logging;
using Foundatio.Caching;

namespace Edelstein.Service.Game.Sockets
{
    public partial class WvsGameSocket : AbstractSocket
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public WvsGame WvsGame { get; }

        public bool IsInstantiated { get; set; }
        public FieldUser FieldUser { get; set; }

        public WvsGameSocket(
            IChannel channel,
            uint seqSend,
            uint seqRecv,
            WvsGame wvsGame
        ) : base(channel, seqSend, seqRecv)
            => WvsGame = wvsGame;

        public override async Task OnDisconnect()
        {
            if (FieldUser == null) return;
            if (!ReadOnlyMode) await OnUpdate();

            var account = FieldUser.Character.Data.Account;
            var state = await WvsGame.AccountStatusCache.GetAsync<AccountState>(account.ID.ToString());

            if (state.HasValue && state.Value != AccountState.MigratingIn)
                await WvsGame.AccountStatusCache.RemoveAsync(account.ID.ToString());

            FieldUser.ConversationContext?.Dispose();
            FieldUser.Field?.Leave(FieldUser);
        }

        public override async Task OnUpdate()
        {
            if (FieldUser == null) return;

            using (var db = WvsGame.DataContextFactory.Create())
            {
                var character = FieldUser.Character;

                character.FieldPortal = (byte) FieldUser.Field.Template.Portals
                    .Values
                    .Where(p => p.Type == FieldPortalType.Spawn)
                    .OrderBy(p =>
                    {
                        var xd = p.Position.X - FieldUser.Position.X;
                        var yd = p.Position.Y - FieldUser.Position.Y;

                        return xd * xd + yd * yd;
                    })
                    .First()
                    .ID;

                db.Update(character);
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