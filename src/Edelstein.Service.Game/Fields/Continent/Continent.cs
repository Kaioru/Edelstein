using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Templates.Server.Continent;
using Edelstein.Core.Utils.Packets;
using Edelstein.Core.Utils.Ticks;
using Edelstein.Network.Packets;
using Edelstein.Provider;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Logging;
using Stateless;
using Stateless.Graph;

namespace Edelstein.Service.Game.Fields.Continent
{
    public class Continent : ITickBehavior
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly StateMachine<ContinentState, ContinentTrigger> _stateMachine;

        public ContinentTemplate Template { get; }
        public ContinentState State => _stateMachine.State;
        public DateTime NextBoarding { get; set; }
        public DateTime? NextEvent { get; set; }
        public bool EventDoing => _stateMachine.State == ContinentState.Event;

        public IField StartShipMoveField { get; }
        public IField WaitField { get; }
        public IField MoveField { get; }
        public IField CabinField { get; }
        public IField EndField { get; }
        public IField EndShipMoveField { get; }

        public Continent(ContinentTemplate template, FieldManager fieldManager)
        {
            _stateMachine = new StateMachine<ContinentState, ContinentTrigger>(ContinentState.Dormant);

            _stateMachine
                .Configure(ContinentState.Dormant)
                .Permit(ContinentTrigger.Board, ContinentState.Wait);
            _stateMachine
                .Configure(ContinentState.Wait)
                .Permit(ContinentTrigger.Start, ContinentState.Move);
            _stateMachine
                .Configure(ContinentState.Move)
                .OnEntryFromAsync(ContinentTrigger.Start, async () =>
                {
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentTarget.TargetStartShipMoveField);
                        p.Encode<byte>((byte) ContinentTrigger.Start);
                        await StartShipMoveField.BroadcastPacket(p);
                    }

                    await Move(WaitField, MoveField);
                })
                .OnExitAsync(async () =>
                {
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentTarget.TargetEndShipMoveField);
                        p.Encode<byte>((byte) ContinentTrigger.End);
                        await EndShipMoveField.BroadcastPacket(p);
                    }

                    await Move(MoveField, EndField);
                    if (CabinField != null)
                        await Move(CabinField, EndField);

                    NextBoarding = NextBoarding.AddMinutes(Template.Term);
                    ResetEvent();
                })
                .Permit(ContinentTrigger.MobGen, ContinentState.Event)
                .Permit(ContinentTrigger.End, ContinentState.Dormant);
            _stateMachine
                .Configure(ContinentState.Event)
                .SubstateOf(ContinentState.Move)
                .OnEntryAsync(async () =>
                {
                    Logger.Debug($"{Template.Info} started the event");

                    // TODO: Mobspawns

                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentTarget.TargetMoveField);
                        p.Encode<byte>((byte) ContinentTrigger.MobGen);
                        await MoveField.BroadcastPacket(p);
                    }
                })
                .OnExitAsync(async () =>
                {
                    Logger.Debug($"{Template.Info} ended the event");

                    // TODO: Mobspawns

                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentTarget.TargetMoveField);
                        p.Encode<byte>((byte) ContinentTrigger.MobDestroy);
                        await MoveField.BroadcastPacket(p);
                    }
                })
                .Permit(ContinentTrigger.MobDestroy, ContinentState.Move);

            _stateMachine.OnTransitioned(t => Logger.Debug(
                $"{Template.Info} continent state transitioned to {t.Destination}")
            );

            Template = template;
            StartShipMoveField = fieldManager.Get(template.StartShipMoveFieldID);
            WaitField = fieldManager.Get(template.WaitFieldID);
            MoveField = fieldManager.Get(template.MoveFieldID);
            if (template.CabinFieldID.HasValue)
                CabinField = fieldManager.Get(template.CabinFieldID.Value);
            EndField = fieldManager.Get(template.EndFieldID);
            EndShipMoveField = fieldManager.Get(template.EndShipMoveFieldID);

            var now = DateTime.UtcNow;

            NextBoarding = now
                .AddMinutes(now.Minute % Template.Term == 0
                    ? 0
                    : Template.Term - now.Minute % Template.Term)
                .AddMinutes(Template.Delay)
                .AddSeconds(-now.Second);
            Logger.Debug($"{Template.Info} continent is scheduled to board at {NextBoarding}");

            ResetEvent();
        }

        private void ResetEvent()
        {
            var random = new Random(
                NextBoarding.Year +
                NextBoarding.Month +
                NextBoarding.Day +
                NextBoarding.Hour +
                NextBoarding.Minute
            );

            if (!Template.Event || random.Next(100) > 30) return;
            NextEvent = NextBoarding
                .AddMinutes(Template.Wait)
                .AddMinutes(random.Next(Template.Required - 5))
                .AddMinutes(2);
            Logger.Debug($"{Template.Info} continent event is scheduled at {NextEvent}");
        }

        private Task Move(IField from, IField to)
        {
            return Task.WhenAll(from
                .GetObjects<IFieldUser>()
                .ToList()
                .Select(u => to.Enter(u, 0)));
        }

        public async Task TryTick()
        {
            var now = DateTime.UtcNow;

            switch (State)
            {
                case ContinentState.Dormant:
                    if (now > NextBoarding)
                        await _stateMachine.FireAsync(ContinentTrigger.Board);
                    break;
                case ContinentState.Wait:
                    if (now > NextBoarding.AddMinutes(Template.Wait))
                        await _stateMachine.FireAsync(ContinentTrigger.Start);
                    break;
                case ContinentState.Move:
                    if (NextEvent.HasValue &&
                        now > NextEvent.Value)
                        await _stateMachine.FireAsync(ContinentTrigger.MobGen);

                    if (now > NextBoarding
                            .AddMinutes(Template.Wait)
                            .AddMinutes(Template.Required))
                        await _stateMachine.FireAsync(ContinentTrigger.End);
                    break;
                case ContinentState.Event:
                    if (now > NextBoarding
                            .AddMinutes(Template.Wait)
                            .AddMinutes(Template.EventEnd))
                        await _stateMachine.FireAsync(ContinentTrigger.MobDestroy);
                    break;
            }
        }
    }
}