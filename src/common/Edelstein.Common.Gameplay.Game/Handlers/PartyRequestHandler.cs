﻿using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Social;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class PartyRequestHandler : AbstractFieldHandler
    {
    private readonly ILogger _logger;
    
    public PartyRequestHandler(ILogger<PartyRequestHandler> logger) => _logger = logger;
    
    public override short Operation => (short)PacketRecvOperations.PartyRequest;
    
    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        var type = (PartyRequestOperations)reader.ReadByte();

        switch (type)
        {
            case PartyRequestOperations.CreateParty:
                return user.StageUser.Context.Pipelines.FieldOnPacketPartyCreateRequest.Process(new FieldOnPacketPartyCreateRequest(
                    user
                ));
            default:
                _logger.LogWarning("Unhandled party request type {Type}", type);
                break;
        }
        
        return Task.CompletedTask;
    }
}
