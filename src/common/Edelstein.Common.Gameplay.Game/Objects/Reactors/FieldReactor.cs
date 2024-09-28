using System;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Reactors;

public class FieldReactor(
    IReactorTemplate template,
    IPoint2D position, 
    bool facingLeft = true
) : AbstractFieldObject(position), IFieldReactor
{
    public override FieldObjectType Type => FieldObjectType.Reactor;
    public IReactorTemplate Template => template;

    public override IDispatchable GetDispatchEnterField(bool isEnterField = false) 
        => new ReactorEnterField
        {
            ObjectID = ObjectID ?? 0,
            TemplateID = template.ID,
            State = 0,
            X = (short)position.X,
            Y = (short)position.Y,
            Flip = facingLeft
        };

    public override IDispatchable GetDispatchLeaveField(bool isLeaveField = false) 
        => new ReactorLeaveField
        {
            ObjectID = ObjectID ?? 0,
            State = 0,
            X = (short)position.X,
            Y = (short)position.Y
        };
}
