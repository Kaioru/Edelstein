using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Contexts;

public interface IGameContextManagers
{
    ICommandManager<IFieldUser> Command { get; }
}
