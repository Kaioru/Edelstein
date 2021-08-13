using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Movements;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects
{
    public interface IFieldLife : IFieldObj
    {
        MoveActionType Action { get; set; }
        IPhysicalLine2D Foothold { get; set; }

        Task Move(IMovePath path);
    }
}
