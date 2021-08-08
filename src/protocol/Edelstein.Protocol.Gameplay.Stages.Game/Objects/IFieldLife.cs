using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldLife : IFieldObj
    {
        MoveActionType Action { get; set; }
        short Foothold { get; set; }

        Task Move(IMovePath path);
    }
}
