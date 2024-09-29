namespace Edelstein.Protocol.Gameplay.Game.Continents.Templates;

public interface IContiMoveTemplateReactor
{
    string Name { get; }
    int StateOnStart { get; }
    int StateOnEnd { get; }
}
