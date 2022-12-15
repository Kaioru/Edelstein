using Edelstein.Protocol.Util.Storages;

namespace Edelstein.Protocol.Gameplay;

public interface IStage : IIdentifiable<string>
{
    Task Enter(IStageUser user);
    Task Leave(IStageUser user);
}
