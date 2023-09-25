using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public class UserPortalScriptRequestHandler : AbstractPipedFieldHandler<FieldOnPacketUserPortalScriptRequest>
{

    public UserPortalScriptRequestHandler(IPipeline<FieldOnPacketUserPortalScriptRequest> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.UserPortalScriptRequest;

    protected override FieldOnPacketUserPortalScriptRequest? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            reader.Skip(1).ReadString(),
            reader.ReadPoint2D()
        );
}
