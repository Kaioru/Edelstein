using System;
using System.Net;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Network.Transport;
using Edelstein.Protocol.Services.Contracts;

namespace Edelstein.Common.Gameplay.Stages
{
    public abstract class AbstractServerStageUser<TStage, TUser, TConfig> : AbstractStageUser<TStage, TUser>, IServerStageUser<TStage, TUser>
        where TStage : AbstractServerStage<TStage, TUser, TConfig>
        where TUser : AbstractServerStageUser<TStage, TUser, TConfig>
        where TConfig : IServerStageInfo
    {
        private static readonly TimeSpan SessionDisconnectDuration = TimeSpan.FromMinutes(3);
        private static readonly TimeSpan SessionUpdateDuration = TimeSpan.FromMinutes(1);

        public long Key { get; set; }

        public bool IsInitialized { get; set; }
        public bool IsMigrating { get; set; }
        public bool IsLoggingIn { get; set; }

        private DateTime LastSentHeartbeatDate { get; set; }
        private DateTime LastRecvHeartbeatDate { get; set; }

        protected AbstractServerStageUser(ISocket socket, IPacketProcessor<TStage, TUser> processor) : base(socket, processor)
        {
            var bytes = new byte[8];
            var random = new Random();

            random.NextBytes(bytes);

            Key = BitConverter.ToInt64(bytes, 0);
        }

        public override async Task Update()
        {
            if (Account != null)
                await Stage.AccountRepository.Update(Account);
            if (AccountWorld != null)
                await Stage.AccountWorldRepository.Update(AccountWorld);
            if (Character != null)
                await Stage.CharacterRepository.Update(Character);
        }

        public override async Task OnDisconnect()
        {
            if (!IsMigrating) await Update();
            await base.OnDisconnect();
        }

        public async Task<bool> MigrateTo(string server)
        {
            var describe = await Stage.ServerRegistry.DescribeByID(new DescribeServerByIDRequest
            {
                Id = server
            });

            return await MigrateTo(describe.Server);
        }

        public async Task<bool> MigrateTo(ServerContract server)
        {
            if (server == null) return false;

            var request = await Stage.MigrationRegistry.Register(new RegisterMigrationRequest
            {
                Migration = new MigrationContract
                {
                    Character = Character.ID,
                    ClientKey = Key,
                    ServerFrom = Stage.ID,
                    ServerTo = server.Id
                }
            });

            if (request.Result != MigrationRegistryResult.Ok) return false;

            IsMigrating = true;

            var session = new SessionContract
            {
                Account = Account.ID,
                Character = Character.ID,
                Server = Stage.ID,
                State = SessionState.Migrating
            };

            await Stage.SessionRegistry.Update(new UpdateSessionRequest { Session = session });

            var endpoint = new IPEndPoint(IPAddress.Parse(server.Host), server.Port);
            var address = endpoint.Address.MapToIPv4().GetAddressBytes();
            var port = endpoint.Port;

            await Update();
            await Dispatch(GetMigratePacket(address, (short)port));
            return true;
        }

        protected virtual IPacket GetMigratePacket(byte[] address, short port)
        {
            var packet = new UnstructuredOutgoingPacket(PacketSendOperations.MigrateCommand);

            packet.WriteBool(true);

            foreach (var b in address) packet.WriteByte(b);
            packet.WriteShort(port);
            return packet;
        }

        public async Task TrySendAliveReq()
        {
            var now = DateTime.UtcNow;

            if (!IsInitialized)
            {
                LastRecvHeartbeatDate = now;
                LastSentHeartbeatDate = now;
                IsInitialized = true;
            }

            if ((now - LastRecvHeartbeatDate) >= SessionDisconnectDuration)
            {
                await Disconnect();
                return;
            }

            if ((now - LastSentHeartbeatDate) >= SessionUpdateDuration)
            {
                LastSentHeartbeatDate = DateTime.UtcNow;
                await Dispatch(GetAliveReqPacket());
            }
        }

        public async Task TryRecvAliveAck()
        {
            if (Account == null) return;
            if ((DateTime.UtcNow - LastRecvHeartbeatDate) < SessionUpdateDuration) return;

            LastRecvHeartbeatDate = DateTime.UtcNow;

            var session = new SessionContract
            {
                Account = Account.ID,
                Server = Stage.ID,
                State = SessionState.LoggedIn
            };

            if (Character != null)
                session.Character = Character.ID;

            var result = (await Stage.SessionRegistry.Update(new UpdateSessionRequest { Session = session })).Result;

            if (result != SessionRegistryResult.Ok) await Disconnect();
        }

        protected virtual IPacket GetAliveReqPacket()
           => new UnstructuredOutgoingPacket(PacketSendOperations.AliveReq);
    }
}
