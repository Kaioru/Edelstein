using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;
using Edelstein.Protocol.Network;

namespace Edelstein.Common.Interop
{
    public class DispatchService<TStage, TUser> : IDispatchService
        where TStage : IStage<TStage, TUser>
        where TUser : IStageUser<TStage, TUser>
    {
        private readonly TStage _stage;

        public DispatchService(TStage stage)
            => _stage = stage;

        public async Task<DispatchResponse> Dispatch(DispatchRequest request)
        {
            var packet = new UnstructuredOutgoingPacket();
            var buffer = request.Packet.ToByteArray();
            var targets = _stage.Users.ToList();

            packet.WriteBytes(buffer);
            await Task.WhenAll(targets.Select(target => target.Dispatch(packet)));

            return new DispatchResponse
            {
                Result = DispatchResult.Ok,
                Reach = targets.Count
            };
        }

        public Task<DispatchResponse> DispatchToAlliance(DispatchToAllianceRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToGuild(DispatchToGuildRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToParty(DispatchToPartyRequest request) { throw new NotImplementedException(); }

        public async Task<DispatchResponse> DispatchToCharacter(DispatchToCharacterRequest request)
        {
            var target = _stage.Users.FirstOrDefault(u => u.Character.ID == request.Character);

            if (target != null)
            {
                var packet = new UnstructuredOutgoingPacket();
                var buffer = request.Packet.ToByteArray();

                packet.WriteBytes(buffer);
                await target.Dispatch(packet);
                return new DispatchResponse
                {
                    Result = DispatchResult.Ok,
                    Reach = 1
                };
            }

            return new DispatchResponse
            {
                Result = DispatchResult.Failed,
                Reach = 0
            };
        }
    }
}
