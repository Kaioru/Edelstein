using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Common.Interop
{
    public class ServerRegistryService : IServerRegistryService
    {
        public Task<ServiceRegistryResponse> RegisterServer(RegisterServerRequest request) { throw new NotImplementedException(); }
        public Task<ServiceRegistryResponse> DeregisterServer(DeregisterServerRequest request) { throw new NotImplementedException(); }
        public Task<ServiceRegistryResponse> UpdateServer(UpdateServerRequest request) { throw new NotImplementedException(); }
        public Task<DescribeServerResponse> DescribeServer(DescribeServerRequest request) { throw new NotImplementedException(); }
        public Task<DescribeServersResponse> DescribeServers(DescribeServersRequest request) { throw new NotImplementedException(); }

        public Task<DispatchResponse> Dispatch(DispatchRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToServer(DispatchToServerRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToServers(DispatchToServersRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToAlliance(DispatchToAllianceRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToGuild(DispatchToGuildRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToParty(DispatchToPartyRequest request) { throw new NotImplementedException(); }
        public Task<DispatchResponse> DispatchToCharacter(DispatchToCharacterRequest request) { throw new NotImplementedException(); }
    }
}
