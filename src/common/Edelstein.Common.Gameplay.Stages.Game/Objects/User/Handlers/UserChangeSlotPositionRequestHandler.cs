using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Gameplay.Inventories;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User.Contracts;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Game.Objects.User.Handlers;

public class UserChangeSlotPositionRequestHandler : AbstractFieldUserHandler
{
    private readonly IPipeline<IUserChangeSlotPositionRequest> _pipeline;

    public UserChangeSlotPositionRequestHandler(IPipeline<IUserChangeSlotPositionRequest> pipeline) =>
        _pipeline = pipeline;

    public override short Operation => (short)PacketRecvOperations.UserChangeSlotPositionRequest;

    protected override Task Handle(IFieldUser user, IPacketReader reader)
    {
        _ = reader.ReadInt();

        var message = new UserChangeSlotPositionRequest(
            user,
            (ItemInventoryType)reader.ReadByte(),
            reader.ReadShort(),
            reader.ReadShort(),
            reader.ReadShort()
        );

        return _pipeline.Process(message);
    }
}
