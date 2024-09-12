using System;
using Edelstein.Common.Gameplay.Game.Objects.Users;
using Edelstein.Common.Utilities.Spatial;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC;

public class FieldNPC(
    INPCTemplate template,
    IPoint2D position,
    IFieldFoothold foothold,
    IRect2D? bounds = null,
    bool facingLeft = true,
    bool enabled = true
) : AbstractFieldLife<IFieldNPCMovePath, IFieldNPCMoveAction>(
        position, 
        foothold, 
        new FieldNPCMoveAction(Convert.ToByte(facingLeft))),
    IFieldNPC
{
    public override FieldObjectType Type => FieldObjectType.NPC;

    public INPCTemplate Template => template;
    public IRect2D Bounds => bounds ?? new Rect2D(position, position);
    public bool IsEnabled => enabled;

    public override IDispatchable GetDispatchEnterField(bool isEnterField = false)
        => new NPCEnterField
        {
            ObjectID = ObjectID ?? 0,
            TemplateID = Template.ID,
            
            X = (short)position.X,
            Y = (short)position.Y,
            MoveAction = Action.Value,
            Foothold = (byte)(Foothold?.ID ?? 0),
            
            RangeMin = (short)Bounds.Left,
            RangeMax = (short)Bounds.Right,
            
            Enabled = IsEnabled
        };

    public override IDispatchable GetDispatchLeaveField(bool isLeaveField = false)
        => new NPCLeaveField
        {
            ObjectID = ObjectID ?? 0
        };
}
