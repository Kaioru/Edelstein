namespace Edelstein.Plugin.Rue.Login.WorldSelect;

public interface IWorldSelectStrategy
{
    Task Execute(WorldSelectContext context);
}
