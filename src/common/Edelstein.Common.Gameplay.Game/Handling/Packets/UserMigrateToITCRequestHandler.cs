﻿using Edelstein.Common.Gameplay.Handling;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserMigrateToITCRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserMigrateToITCRequest>
{
    public UserMigrateToITCRequestHandler(IPipeline<FieldOnPacketUserMigrateToITCRequest> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserMigrateToITCRequest;

    protected override FieldOnPacketUserMigrateToITCRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(user);
}
