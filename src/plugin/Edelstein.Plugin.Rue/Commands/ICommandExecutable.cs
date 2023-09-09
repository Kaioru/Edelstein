using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands;

public interface ICommandExecutable
{
    bool Check(IFieldUser user);
    Task Execute(IFieldUser user, string[] args);
}
