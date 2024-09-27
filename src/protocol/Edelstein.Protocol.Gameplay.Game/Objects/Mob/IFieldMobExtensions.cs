namespace Edelstein.Protocol.Gameplay.Game.Objects.Mob;

public static class IFieldMobExtensions
{
    public static StructuredMobInfo ToStructured(this IFieldMob mob)
        => new()
        {
            TemplateID = mob.Template.ID,

            X = (short)mob.Position.X,
            Y = (short)mob.Position.Y,
            MoveAction = mob.Action.Value,
            Fh = (short)(mob.Foothold?.ID ?? 0),
            FhStart =  (short)(mob.FootholdStart?.ID ?? 0),
        };
}
