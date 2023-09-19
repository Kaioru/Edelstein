using Edelstein.Protocol.Services.Social.Contracts;

namespace Edelstein.Protocol.Services.Social;

public interface IPartyService
{
    Task<PartyLoadResponse> Load(PartyLoadRequest request);

    Task<PartyResponse> UpdateChannelOrField(PartyUpdateChannelOrFieldRequest request);
    Task<PartyResponse> UpdateLevelOrJob(PartyUpdateLevelOrJobRequest request);
}
