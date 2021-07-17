using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public interface IContiMove : IFieldPool, IRepositoryEntry<int>
    {
        ContiMoveState State { get; }

        string Info { get; }

        DateTime NextBoarding { get; }
        DateTime NextEvent { get; }
        bool IsEventActive { get; }

        IField StartShipMoveField { get; }
        IField WaitField { get; }
        IField MoveField { get; }
        IField CabinField { get; }
        IField EndField { get; }
        IField EndShipMoveField { get; }

        Task Trigger(ContiMoveStateTrigger trigger);
        Task Dispatch(ContiMoveTarget target, IPacket packet);
    }
}
