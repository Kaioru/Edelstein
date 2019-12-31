using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Movements;

namespace Edelstein.Service.Game.Fields.Objects
{
    public interface IFieldLife : IFieldObj
    {
        byte MoveAction { get; set; }
        short Foothold { get; set; }

        public Task Move(IMovePath path);
    }
}