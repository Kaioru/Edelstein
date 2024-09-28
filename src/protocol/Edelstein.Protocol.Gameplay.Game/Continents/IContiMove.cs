using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Continents.Templates;

namespace Edelstein.Protocol.Gameplay.Game.Continents;

public interface IContiMove : IFieldSet
{
    IContiMoveTemplate Template { get; }

    ContiMoveState State { get; }

    IField StartShipMoveField { get; }
    IField WaitField { get; }
    IField MoveField { get; }
    IField? CabinField { get; }
    IField EndField { get; }
    IField EndShipMoveField { get; }

    DateTime NextBoarding { get; }
    DateTime NextStart { get; }
    DateTime NextEnd { get; }
    DateTime? NextEvent { get; }
    DateTime? NextEventEnd { get; }

    Task Trigger(ContiMoveStateTrigger trigger);
}
