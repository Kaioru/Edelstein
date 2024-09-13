namespace Edelstein.Protocol.Gameplay.Game.Objects.NPC;

public static class IFieldNPCExtensions
{
    public static StructuredNPCInfo ToStructured(this IFieldNPC npc)
        => new()
        {
            TemplateID = npc.Template.ID,

            X = (short)npc.Position.X,
            Y = (short)npc.Position.Y,
            MoveAction = npc.Action.Value,
            Foothold = (short)(npc.Foothold?.ID ?? 0),

            RangeMin = (short)npc.Bounds.Left,
            RangeMax = (short)npc.Bounds.Right,

            Enabled = npc.IsEnabled
        };
}
