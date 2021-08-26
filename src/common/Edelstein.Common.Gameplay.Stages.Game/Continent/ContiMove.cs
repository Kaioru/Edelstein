using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Stages.Game.Continent.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Continent;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Network;
using Edelstein.Protocol.Util.Ticks;
using Microsoft.Extensions.Logging;
using Stateless;

namespace Edelstein.Common.Gameplay.Stages.Game.Continent
{
    public class ContiMove : IContiMove, ITickerBehavior
    {
        private readonly GameStage _gameStage;
        private readonly ContiMoveTemplate _template;
        private readonly StateMachine<ContiMoveState, ContiMoveStateTrigger> _stateMachine;

        public int ID => _template.ID;
        public IContiMoveInfo Info => _template;

        public ContiMoveState State => _stateMachine.State;

        public IField StartShipMoveField { get; }
        public IField WaitField { get; }
        public IField MoveField { get; }
        public IField CabinField { get; }
        public IField EndField { get; }
        public IField EndShipMoveField { get; }

        private DateTime _nextBoarding;
        private DateTime? _nextEvent;

        public ContiMove(
            GameStage gameStage,
            ContiMoveTemplate template,
            IFieldRepository fieldRepository
        )
        {
            _gameStage = gameStage;
            _template = template;
            _stateMachine = new StateMachine<ContiMoveState, ContiMoveStateTrigger>(ContiMoveState.Dormant);

            _stateMachine
                .Configure(ContiMoveState.Dormant)
                .Permit(ContiMoveStateTrigger.Board, ContiMoveState.Wait);
            _stateMachine
                .Configure(ContiMoveState.Wait)
                .Permit(ContiMoveStateTrigger.Start, ContiMoveState.Move);
            _stateMachine
                .Configure(ContiMoveState.Move)
                .OnEntryFromAsync(ContiMoveStateTrigger.Start, async () =>
                {
                    await Move(WaitField, MoveField);
                    await StartShipMoveField.Dispatch(
                        new UnstructuredOutgoingPacket(PacketSendOperations.CONTIMOVE)
                            .WriteByte((byte)ContiMoveTarget.TargetStartShipMoveField)
                            .WriteByte((byte)ContiMoveStateTrigger.Start)
                    );
                })
                .OnExitAsync(async () =>
                {
                    await Move(MoveField, EndField);
                    if (CabinField != null)
                        await Move(CabinField, EndField);

                    await EndShipMoveField.Dispatch(
                        new UnstructuredOutgoingPacket(PacketSendOperations.CONTIMOVE)
                            .WriteByte((byte)ContiMoveTarget.TargetEndShipMoveField)
                            .WriteByte((byte)ContiMoveStateTrigger.End)
                    );

                    _nextBoarding = _nextBoarding.AddMinutes(_template.Term);
                    ResetEvent();
                })
                .Permit(ContiMoveStateTrigger.MobGen, ContiMoveState.Event)
                .Permit(ContiMoveStateTrigger.End, ContiMoveState.Dormant);
            _stateMachine
                .Configure(ContiMoveState.Event)
                .SubstateOf(ContiMoveState.Move)
                .OnEntryAsync(async () =>
                {
                    _nextEvent = null;
                    _gameStage.Logger.LogDebug($"{_template.Name} started the event");

                    // TODO: Mobspawns

                    await MoveField.Dispatch(
                        new UnstructuredOutgoingPacket(PacketSendOperations.CONTIMOVE)
                            .WriteByte((byte)ContiMoveTarget.TargetMoveField)
                            .WriteByte((byte)ContiMoveStateTrigger.MobGen)
                    );
                })
                .OnExitAsync(async () =>
                {
                    _gameStage.Logger.LogDebug($"{_template.Name} ended the event");

                    // TODO: Mobspawns

                    await MoveField.Dispatch(
                        new UnstructuredOutgoingPacket(PacketSendOperations.CONTIMOVE)
                            .WriteByte((byte)ContiMoveTarget.TargetMoveField)
                            .WriteByte((byte)ContiMoveStateTrigger.MobDestroy)
                    );
                })
                .Permit(ContiMoveStateTrigger.MobDestroy, ContiMoveState.Move);

            _stateMachine.OnTransitioned(t => _gameStage.Logger.LogDebug(
                $"{_template.Name} contimove state transitioned to {t.Destination}")
            );

            StartShipMoveField = fieldRepository.Retrieve(_template.StartShipMoveFieldID).Result;
            WaitField = fieldRepository.Retrieve(_template.WaitFieldID).Result;
            MoveField = fieldRepository.Retrieve(_template.MoveFieldID).Result;
            if (_template.CabinFieldID.HasValue)
                CabinField = fieldRepository.Retrieve(_template.CabinFieldID.Value).Result;
            EndField = fieldRepository.Retrieve(_template.EndFieldID).Result;
            EndShipMoveField = fieldRepository.Retrieve(_template.EndShipMoveFieldID).Result;

            var now = DateTime.UtcNow;

            _nextBoarding = now
                .AddMinutes(now.Minute % _template.Term == 0
                    ? 0
                    : _template.Term - now.Minute % _template.Term)
                .AddMinutes(_template.Delay)
                .AddSeconds(-now.Second);
            _gameStage.Logger.LogDebug($"{_template.Name} contimove is scheduled to board at {_nextBoarding}");

            ResetEvent();
        }

        private void ResetEvent()
        {
            var random = new Random(
                _nextBoarding.Year +
                _nextBoarding.Month +
                _nextBoarding.Day +
                _nextBoarding.Hour +
                _nextBoarding.Minute
            );

            if (!_template.Event || random.Next(100) > 30) return;
            _nextEvent = _nextBoarding
                .AddMinutes(_template.Wait)
                .AddMinutes(random.Next(_template.Required - 5))
                .AddMinutes(2);
            _gameStage.Logger.LogDebug(
                $"{_template.Name} contimove event is scheduled at " +
                $"{_nextEvent} to " +
                $"{_nextBoarding.AddMinutes(_template.Wait).AddMinutes(_template.EventEnd)}"
            );
        }

        private Task Move(IField from, IField to)
        {
            return Task.WhenAll(from
                .GetObjects<IFieldObjUser>()
                .Select(u => to.Enter(u, 0)));
        }

        public async Task OnTick(DateTime now)
        {
            if (_stateMachine.IsInState(ContiMoveState.Dormant))
            {
                if (now > _nextBoarding)
                    await Trigger(ContiMoveStateTrigger.Board);
            }

            if (_stateMachine.IsInState(ContiMoveState.Wait))
            {
                if (now > _nextBoarding.AddMinutes(_template.Wait))
                    await Trigger(ContiMoveStateTrigger.Start);
            }

            if (_stateMachine.IsInState(ContiMoveState.Move))
            {
                if (now > _nextBoarding
                        .AddMinutes(_template.Wait)
                        .AddMinutes(_template.Required))
                    await Trigger(ContiMoveStateTrigger.End);
            }

            if (State == ContiMoveState.Move)
                if (_nextEvent.HasValue && now > _nextEvent.Value)
                    await Trigger(ContiMoveStateTrigger.MobGen);

            if (State == ContiMoveState.Event)
                if (now > _nextBoarding
                        .AddMinutes(_template.Wait)
                        .AddMinutes(_template.EventEnd))
                    await Trigger(ContiMoveStateTrigger.MobDestroy);
        }

        public Task Trigger(ContiMoveStateTrigger trigger) => _stateMachine.FireAsync(trigger);
        public Task Enter(IFieldObjUser user)
            => (State switch
            {
                ContiMoveState.Wait => WaitField,
                ContiMoveState.Move => MoveField,
                ContiMoveState.Event => MoveField,
                _ => StartShipMoveField
            }).Enter(user);
    }
}
