using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Field.Continent;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Continents
{
    public class Continent : ITickable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public ContinentTemplate Template { get; }
        public ContinentState State { get; set; } = ContinentState.Dormant;
        public DateTime NextBoarding { get; set; }
        public DateTime? NextEvent { get; set; }
        public bool EventDoing { get; set; }

        private IField StartShipMoveField { get; }
        public IField WaitField { get; }
        public IField MoveField { get; }
        public IField? CabinField { get; }
        public IField EndField { get; }
        public IField EndShipMoveField { get; }

        public Continent(FieldManager fieldManager, ContinentTemplate template)
        {
            Template = template;
            StartShipMoveField = fieldManager.Get(template.StartShipMoveFieldID);
            WaitField = fieldManager.Get(template.WaitFieldID);
            MoveField = fieldManager.Get(template.MoveFieldID);
            if (template.CabinFieldID.HasValue)
                CabinField = fieldManager.Get(template.CabinFieldID.Value);
            EndField = fieldManager.Get(template.EndFieldID);
            EndShipMoveField = fieldManager.Get(template.EndShipMoveFieldID);

            var now = DateTime.Now;

            NextBoarding = now
                .AddMinutes(now.Minute % Template.Term == 0
                    ? 0
                    : Template.Term - now.Minute % Template.Term)
                .AddMinutes(Template.Delay)
                .AddSeconds(-now.Second);
            ResetEvent();
        }

        private Task Move(IField from, IField to)
        {
            return Task.WhenAll(from
                .GetObjects<IFieldUser>()
                .ToList()
                .Select(u => to.Enter(u, 0)));
        }

        private void ResetEvent()
        {
            var random = new Random();

            if (Template.Event &&
                random.Next(100) <= 30
            )
            {
                NextEvent = NextBoarding
                    .AddMinutes(Template.Wait)
                    .AddMinutes(random.Next(Template.Required - 5))
                    .AddMinutes(2);
            }
        }

        public async Task OnTick(DateTime now)
        {
            var eventState = State;
            var nextState = State;

            switch (State)
            {
                case ContinentState.Dormant:
                    if ((now - NextBoarding).Seconds > 0)
                    {
                        eventState = ContinentState.Wait;
                        nextState = ContinentState.Wait;
                    }

                    break;
                case ContinentState.Wait:
                    if ((now - NextBoarding
                             .AddMinutes(Template.Wait)).Seconds > 0)
                    {
                        eventState = ContinentState.Start;
                        nextState = ContinentState.Move;
                    }

                    break;
                case ContinentState.Move:
                    if (NextEvent.HasValue)
                    {
                        if (!EventDoing &&
                            (now - NextEvent.Value).Seconds > 0)
                            eventState = ContinentState.MobGen;

                        if (EventDoing &&
                            (now - NextBoarding
                                 .AddMinutes(Template.Wait)
                                 .AddMinutes(Template.EventEnd)).Seconds > 0)
                            eventState = ContinentState.MobDestroy;
                    }

                    if ((now - NextBoarding
                             .AddMinutes(Template.Wait)
                             .AddMinutes(Template.Required)).Seconds > 0)
                    {
                        eventState = ContinentState.End;
                        nextState = ContinentState.Dormant;
                    }

                    break;
            }

            if (State != nextState)
            {
                State = nextState;
                Logger.Debug($"{Template.Info} continent state has been updated to {State}");
            }

            switch (eventState)
            {
                case ContinentState.Start:
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetStartShipMoveField);
                        p.Encode<byte>((byte) ContinentState.Start);
                        await StartShipMoveField.BroadcastPacket(p);
                    }

                    await Move(WaitField, MoveField);
                    break;
                case ContinentState.MobGen:
                    NextEvent = null;
                    EventDoing = true;
                    Logger.Debug($"{Template.Info} started the {nextState} event");
                    break;
                case ContinentState.MobDestroy:
                    EventDoing = false;
                    Logger.Debug($"{Template.Info} started the {nextState} event");
                    break;
                case ContinentState.End:
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetEndShipMoveField);
                        p.Encode<byte>((byte) ContinentState.End);
                        await EndShipMoveField.BroadcastPacket(p);
                    }

                    await Move(MoveField, EndField);
                    if (CabinField != null)
                        await Move(CabinField, EndField);

                    NextBoarding = NextBoarding.AddMinutes(Template.Term);
                    ResetEvent();
                    break;
            }
        }
    }
}