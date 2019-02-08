using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates.Server.Continent;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Continent
{
    public class Continent : IUpdateable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        public ContinentTemplate Template { get; }
        private readonly FieldManager _fieldManager;

        public ContinentState State { get; set; }
        private DateTime NextBoarding { get; set; }

        public Continent(ContinentTemplate template, FieldManager fieldManager)
        {
            Template = template;
            _fieldManager = fieldManager;

            var now = DateTime.Now;

            NextBoarding = now
                .AddMinutes(now.Minute % Template.Term == 0
                    ? 0
                    : Template.Term - now.Minute % Template.Term)
                .AddMinutes(Template.Delay)
                .AddSeconds(-now.Second);
        }

        private Task MoveField(int from, int to)
        {
            var fromField = _fieldManager.Get(from);
            var toField = _fieldManager.Get(to);

            return Task.WhenAll(fromField
                .GetObjects<FieldUser>()
                .ToList()
                .Select(u => toField.Enter(u, 0)));
        }

        private Task BroadcastPacket(int to, IPacket p)
        {
            var toField = _fieldManager.Get(to);
            return toField.BroadcastPacket(p);
        }

        public async Task OnUpdate(DateTime now)
        {
            var currentState = State;
            var newState = currentState;

            switch (currentState)
            {
                case ContinentState.Dormant:
                    if ((now - NextBoarding).Seconds > 0)
                        newState = ContinentState.Wait;
                    break;
                case ContinentState.Wait:
                    if ((now - NextBoarding
                             .AddMinutes(Template.Wait)).Seconds > 0)
                        newState = ContinentState.Move;
                    break;
                case ContinentState.Move:
                    if ((now - NextBoarding
                             .AddMinutes(Template.Wait)
                             .AddMinutes(Template.Required)).Seconds > 0)
                    {
                        newState = ContinentState.Dormant;
                        NextBoarding = NextBoarding.AddMinutes(Template.Term);
                    }

                    break;
            }

            if (newState == currentState) return;

            State = newState;
            Logger.Debug($"{Template.Info} continent state has been updated to {State}");

            switch (State)
            {
                case ContinentState.Dormant:
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetEndShipMoveField);
                        p.Encode<byte>((byte) ContinentState.End);
                        await BroadcastPacket(Template.EndShipMoveFieldID, p);
                    }
                    
                    await MoveField(Template.MoveFieldID, Template.EndFieldID);
                    if (Template.CabinFieldID.HasValue)
                        await MoveField(Template.CabinFieldID.Value, Template.EndFieldID);
                    break;
                case ContinentState.Wait:
                    break;
                case ContinentState.Move:
                    using (var p = new Packet(SendPacketOperations.CONTIMOVE))
                    {
                        p.Encode<byte>((byte) ContinentState.TargetStartShipMoveField);
                        p.Encode<byte>((byte) ContinentState.Start);
                        await BroadcastPacket(Template.StartShipMoveFieldID, p);
                    }

                    await MoveField(Template.WaitFieldID, Template.MoveFieldID);
                    break;
            }
        }
    }
}