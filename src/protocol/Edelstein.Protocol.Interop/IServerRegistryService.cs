﻿using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Interop.Contracts;

namespace Edelstein.Protocol.Interop
{
    [ServiceContract]
    public interface IServerRegistryService
    {
        [OperationContract] Task<ServiceRegistryResponse> RegisterServer(RegisterServerRequest request);
        [OperationContract] Task<ServiceRegistryResponse> DeregisterServer(DeregisterServerRequest request);
        [OperationContract] Task<ServiceRegistryResponse> UpdateServer(UpdateServerRequest request);
        [OperationContract] Task<DescribeServerResponse> DescribeServer(DescribeServerRequest request);
        [OperationContract] Task<DescribeServersResponse> DescribeServers(DescribeServersRequest request);

        [OperationContract] Task<DispatchResponse> Dispatch(DispatchRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToServer(DispatchToServerRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToServers(DispatchToServersRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToAlliance(DispatchToAllianceRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToGuild(DispatchToGuildRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToParty(DispatchToPartyRequest request);
        [OperationContract] Task<DispatchResponse> DispatchToCharacter(DispatchToCharacterRequest request);
    }
}
