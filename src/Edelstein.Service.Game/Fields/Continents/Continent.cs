using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Core.Utils;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field.Continent;
using Edelstein.Provider.Templates.Field.Life.Mob;
using Edelstein.Provider.Templates.Item.Consume;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.Objects.Mob;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Continents
{
    public class Continent : ITickable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly ITemplateManager _templateManager;

        public ContinentTemplate Template { get; }
        public ContinentState State { get; set; } = ContinentState.Dormant;
        public DateTime NextBoarding { get; set; }
        public DateTime? NextEvent { get; set; }
        public bool EventDoing { get; set; }

        public IField StartShipMoveField { get; }
        public IField WaitField { get; }
        public IField MoveField { get; }
        public IField? CabinField { get; }
        public IField EndField { get; }
        public IField EndShipMoveField { get; }

        public Continent(ITemplateManager templateManager, FieldManager fieldManager, ContinentTemplate template)
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

        public async Task OnTick(DateTime now)
        {
            var eventState = State;
            var nextState = State;

            switch (State)
            {
                case ContinentState.Dormant:
                    if (now > NextBoarding)
                    {
                        eventState = ContinentState.Wait;
                        nextState = ContinentState.Wait;
                    }

                    break;
                case ContinentState.Wait:
                    if (now > NextBoarding.AddMinutes(Template.Wait))
                    {
                        eventState = ContinentState.Start;
                        nextState = ContinentState.Move;
                    }

                    break;
                case ContinentState.Move:
                    if (NextEvent.HasValue &&
                        !EventDoing &&
                        now > NextEvent.Value)
                        eventState = ContinentState.MobGen;
                    if (EventDoing &&
                        (now > NextBoarding
                             .AddMinutes(Template.Wait)
                             .AddMinutes(Template.EventEnd) ||
                         MoveField.GetObjects<FieldMob>().Any()))
                        eventState = ContinentState.MobDestroy;
                    if (now > NextBoarding
                            .AddMinutes(Template.Wait)
                            .AddMinutes(Template.Required))
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
                    Logger.Debug($"{Template.Info} started the {eventState} event");

                    if (Template.GenMob != null)
                    {
                        await Task.WhenAll(_templateManager
                            .Get<MobSummonItemTemplate>(Template.GenMob.TemplateID).Mobs
                            .Select(m => _templateManager.Get<MobTemplate>(m.TemplateID))
                            .Select(m => MoveField.Enter(new FieldMob(m)
                            {
                                Position = Template.GenMob.Position
                            })));
                    }

                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetMoveField);
                        p.Encode<byte>((byte) ContinentState.MobGen);
                        await MoveField.BroadcastPacket(p);
                    }

                    break;
                case ContinentState.MobDestroy:
                    EventDoing = false;
                    Logger.Debug($"{Template.Info} started the {eventState} event");

                    await Task.WhenAll(MoveField
                        .GetObjects<FieldMob>()
                        .Select(m => MoveField.Leave(m)));

                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetMoveField);
                        p.Encode<byte>((byte) ContinentState.MobDestroy);
                        await MoveField.BroadcastPacket(p);
                    }

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