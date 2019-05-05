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

        private readonly FieldManager _fieldManager;

        public ContinentTemplate Template { get; }
        public ContinentState State { get; set; } = ContinentState.Dormant;
        public DateTime NextBoarding { get; set; }
        public DateTime? NextEvent { get; set; }
        public bool EventDoing { get; set; }

        public Continent(FieldManager fieldManager, ContinentTemplate template)
        {
            _fieldManager = fieldManager;
            Template = template;

            var now = DateTime.Now;

            NextBoarding = now
                .AddMinutes(now.Minute % Template.Term == 0
                    ? 0
                    : Template.Term - now.Minute % Template.Term)
                .AddMinutes(Template.Delay)
                .AddSeconds(-now.Second);
            ResetEvent();
        }

        private Task MoveField(int from, int to)
        {
            var fromField = _fieldManager.Get(from);
            var toField = _fieldManager.Get(to);

            return Task.WhenAll(fromField
                .GetObjects<IFieldUser>()
                .ToList()
                .Select(u => toField.Enter(u, 0)));
        }

        private Task BroadcastPacket(int to, IPacket p)
        {
            var toField = _fieldManager.Get(to);
            return toField.BroadcastPacket(p);
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
                        await BroadcastPacket(Template.StartShipMoveFieldID, p);
                    }

                    await MoveField(Template.WaitFieldID, Template.MoveFieldID);
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
                        await BroadcastPacket(Template.EndShipMoveFieldID, p);
                    }

                    await MoveField(Template.MoveFieldID, Template.EndFieldID);
                    if (Template.CabinFieldID.HasValue)
                        await MoveField(Template.CabinFieldID.Value, Template.EndFieldID);

                    NextBoarding = NextBoarding.AddMinutes(Template.Term);
                    ResetEvent();
                    break;
            }
        }
    }
}