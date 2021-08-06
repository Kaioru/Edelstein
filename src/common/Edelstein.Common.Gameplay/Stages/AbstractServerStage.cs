using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Behaviors;
using Edelstein.Common.Gameplay.Stages.Handlers;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Gameplay.Users;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Ticks;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractServerStage<TStage, TUser, TConfig> : AbstractStage<TStage, TUser>, IServerStage<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : ServerStageConfig
    {
        private static readonly TimeSpan ServerUpdateFreq = TimeSpan.FromMinutes(1);
        private static readonly TimeSpan AliveBehaviorFreq = TimeSpan.FromSeconds(1);

        public ServerStageType Type { get; init; }
        public TConfig Config { get; init; }
        public string ID => Config.ID;

        public IServerRegistryService ServerRegistryService { get; init; }
        public ISessionRegistryService SessionRegistry { get; init; }
        public IMigrationRegistryService MigrationRegistryService { get; init; }

        public IAccountRepository AccountRepository { get; init; }
        public IAccountWorldRepository AccountWorldRepository { get; init; }
        public ICharacterRepository CharacterRepository { get; init; }

        public Task DispatchSubscriptionTask { get; set; }

        protected AbstractServerStage(
            ServerStageType type,
            TConfig config,
            IServerRegistryService serverRegistryService,
            ISessionRegistryService sessionRegistry,
            IMigrationRegistryService migrationRegistryService,
            IAccountRepository accountRepository,
            IAccountWorldRepository accountWorldRepository,
            ICharacterRepository characterRepository,
            ITickerManager timerManager,
            IPacketProcessor<TStage, TUser> processor
        ) : base()
        {
            Type = type;
            Config = config;
            ServerRegistryService = serverRegistryService;
            SessionRegistry = sessionRegistry;
            MigrationRegistryService = migrationRegistryService;
            AccountRepository = accountRepository;
            AccountWorldRepository = accountWorldRepository;
            CharacterRepository = characterRepository;

            timerManager.Schedule(new ServerUpdateBehavior<TStage, TUser, TConfig>((TStage)this), ServerUpdateFreq, TimeSpan.Zero);
            timerManager.Schedule(new AliveReqBehavior<TStage, TUser, TConfig>((TStage)this), AliveBehaviorFreq);
            processor.Register(new AliveAckHandler<TStage, TUser, TConfig>());
            processor.Register(new MigrateInHandler<TStage, TUser, TConfig>((TStage)this));
        }

        public override async Task Enter(TUser user)
        {
            var session = new SessionObject
            {
                Server = ID,
                State = user.IsLoggingIn ? SessionState.LoggingIn : SessionState.LoggedIn
            };

            if (user.Account != null) session.Account = user.Account.ID;
            if (user.Character != null) session.Character = user.Character.ID;

            await SessionRegistry.UpdateSession(new UpdateSessionRequest { Session = session });
            await base.Enter(user);
        }

        public override async Task Leave(TUser user)
        {
            if (user.Account != null && !user.IsMigrating)
            {
                var session = new SessionObject
                {
                    Account = user.Account.ID,
                    State = SessionState.Offline
                };

                await SessionRegistry.UpdateSession(new UpdateSessionRequest { Session = session });
            }
            await base.Leave(user);
        }

        public Task OnNotifyDispatch(DispatchObject dispatch)
        {
            var targets = new List<TUser>();
            var packet = new UnstructuredOutgoingPacket();

            packet.WriteBytes(dispatch.Packet.ToByteArray());

            if (
                dispatch.HasAlliance ||
                dispatch.HasGuild ||
                dispatch.HasParty ||
                dispatch.HasCharacter
            )
            {
                if (dispatch.HasCharacter && GetUser(dispatch.Character) != null)
                    targets.Add(GetUser(dispatch.Character));
            }
            else targets.AddRange(GetUsers());

            return Task.WhenAll(targets.Select(t => t.Dispatch(packet)));
        }
    }
}
