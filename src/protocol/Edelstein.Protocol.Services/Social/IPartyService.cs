﻿using System.Collections.Generic;
using System.ServiceModel;
using System.Threading.Tasks;
using Edelstein.Protocol.Services.Contracts.Social;
using ProtoBuf.Grpc;

namespace Edelstein.Protocol.Services.Social
{
    [ServiceContract]
    public interface IPartyService
    {
        [OperationContract] Task<PartyLoadByIDResponse> LoadByID(PartyLoadByIDRequest request);
        [OperationContract] Task<PartyLoadByCharacterResponse> LoadByCharacter(PartyLoadByCharacterRequest request);

        [OperationContract] Task<PartyCreateResponse> Create(PartyCreateRequest request);
        [OperationContract] Task<PartyWithdrawResponse> Withdraw(PartyWithdrawRequest request);
        [OperationContract] Task<PartyJoinResponse> Join(PartyJoinRequest request);

        [OperationContract] IAsyncEnumerable<PartyUpdateContract> Subscribe(CallContext context = default);
    }
}
