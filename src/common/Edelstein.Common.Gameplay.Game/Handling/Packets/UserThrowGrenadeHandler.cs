using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public class UserThrowGrenadeHandler : AbstractPipedFieldHandler<FieldOnPacketUserThrowGrenade>
{
    public UserThrowGrenadeHandler(IPipeline<FieldOnPacketUserThrowGrenade> pipeline) : base(pipeline)
    {
    }

    public override short Operation => (short)PacketRecvOperations.UserThrowGrenade;

    protected override FieldOnPacketUserThrowGrenade? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            new Point2D(reader.ReadInt(), reader.ReadInt()),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt()
        );
}
