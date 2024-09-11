using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Templates.Spatial;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Protocol.Gameplay.Game.Objects;

public interface IFieldLife : IFieldObject
{
    Task UpdatePosition(IFieldPortal portal);
    Task UpdatePosition();
}
