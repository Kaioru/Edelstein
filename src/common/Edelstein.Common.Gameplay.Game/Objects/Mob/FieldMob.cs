using System;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob;

public class FieldMob(
    IMobTemplate template,
    IPoint2D position,
    IFieldFoothold? foothold = null,
    IFieldFoothold? footholdStart = null,
    bool facingLeft = true
) : AbstractFieldLifeControllable<IFieldMobMovePath, IFieldMobMoveAction>(
        position,
        foothold,
        new FieldMobMoveAction(template.MoveAbility, facingLeft)),
    IFieldMob
{
    public override FieldObjectType Type => FieldObjectType.Mob;
    public IMobTemplate Template => template;
    
    public IFieldFoothold? FootholdStart => footholdStart;

    public override IDispatchable GetDispatchEnterField(bool isEnterField = false) 
        => new MobEnterField
        {
            ObjectID = ObjectID ?? 0,
            Info = this.ToStructured()
        };

    public override IDispatchable GetDispatchLeaveField(bool isLeaveField = false)
        => new MobLeaveField
        {
            ObjectID = ObjectID ?? 0,
            LeaveType = FieldMobLeaveType.None
        };

    public override IDispatchable GetDispatchChangeController(IFieldObjectController? controller = null)
        => new MobChangeController
        {
            Controller = controller != null,
            ObjectID = ObjectID ?? 0,
            Info = controller != null ? this.ToStructured() : null
        };
}
