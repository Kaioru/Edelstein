using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Movements;

namespace Edelstein.Service.Game.Fields.Objects
{
    public abstract class AbstractFieldLife : AbstractFieldObj, IFieldLife
    {
        public byte MoveAction { get; set; }
        public short Foothold { get; set; }

        public async Task Move(IMovePath path)
        {
            var context = path.Apply(this);

            if (context.Position.HasValue)
                await UpdateFieldSplit();
        }
    }
}