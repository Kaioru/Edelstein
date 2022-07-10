using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC
{
    public interface IFieldObjNPC : IFieldControlledObj, IFieldLife
    {
        IFieldObjNPCInfo Info { get; }

        Task Talk(IFieldObjUser user);
    }
}
