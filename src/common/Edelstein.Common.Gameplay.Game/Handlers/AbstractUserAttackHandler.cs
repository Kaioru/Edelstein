using Edelstein.Common.Gameplay.Game.Combat;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Game.Combat;
using Edelstein.Protocol.Gameplay.Game.Contracts;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handlers;

public abstract class AbstractUserAttackHandler : AbstractPipedFieldHandler<FieldOnPacketUserAttack>
{
    protected abstract AttackType Type { get; }
    
    protected AbstractUserAttackHandler(IPipeline<FieldOnPacketUserAttack?> pipeline) : base(pipeline)
    {
    }

    protected override FieldOnPacketUserAttack? Serialize(IFieldUser user, IPacketReader reader)
        => new(
            user,
            reader.Read(new AttackRequest(Type))
        );
}
