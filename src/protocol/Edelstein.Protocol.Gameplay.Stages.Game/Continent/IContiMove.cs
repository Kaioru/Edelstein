using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public interface IContiMove : IRepositoryEntry<int>
    {
        IContiMoveInfo Info { get; }

        ContiMoveState State { get; }

        IField StartShipMoveField { get; }
        IField WaitField { get; }
        IField MoveField { get; }
        IField CabinField { get; }
        IField EndField { get; }
        IField EndShipMoveField { get; }

        Task Trigger(ContiMoveStateTrigger trigger);
        Task Enter(IFieldObjUser user);
    }
}
