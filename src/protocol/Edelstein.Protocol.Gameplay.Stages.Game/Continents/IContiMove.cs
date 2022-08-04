using Edelstein.Protocol.Gameplay.Stages.Game.Continents.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Continents;

public interface IContiMove : IIdentifiable<int>, IFieldObjectPool
{
    IContiMoveTemplate Template { get; }

    ContiMoveState State { get; }

    IField StartShipMoveField { get; }
    IField WaitField { get; }
    IField MoveField { get; }
    IField CabinField { get; }
    IField EndField { get; }
    IField EndShipMoveField { get; }

    Task Trigger(ContiMoveState trigger);
}
