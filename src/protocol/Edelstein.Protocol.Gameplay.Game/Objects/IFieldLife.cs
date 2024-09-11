using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Movements;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldLife<in TMovePath, TMoveAction> : IFieldObject
    where TMoveAction : IMoveAction
{
    TMoveAction Action { get; set; }
    
    Task UpdatePosition(IFieldPortal portal);
    Task UpdatePosition(TMovePath path);
}
