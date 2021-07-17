using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continent
{
    public interface IContiMove : IPacketDispatcher, IRepositoryEntry<int>
    {
        ContiMoveTemplate Template { get; }

        ContiMoveState State { get; }

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
