using System;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Network.Packet;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Server.Continent;
using Edelstein.Service.Game.Fields.User;
using Edelstein.Service.Game.Logging;

namespace Edelstein.Service.Game.Fields.Continent
{
    public class Continent : IUpdateable
    {
        private static readonly ILog Logger = LogProvider.GetCurrentClassLogger();

        private readonly ContinentTemplate _template;
        private readonly FieldManager _fieldManager;

        private ContinentState State { get; set; }

        private DateTime NextBoarding { get; set; }

        public Continent(ContinentTemplate template, FieldManager fieldManager)
        {
            _template = template;
            _fieldManager = fieldManager;

            var now = DateTime.Now;

            NextBoarding = now
                .AddMinutes(now.Minute % _template.Term == 0
                    ? 0
                    : _template.Term - now.Minute % _template.Term)
                .AddMinutes(_template.Delay)
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
                             .AddMinutes(_template.Wait)).Seconds > 0)
                        newState = ContinentState.Move;
                    break;
                case ContinentState.Move:
                    if ((now - NextBoarding
                             .AddMinutes(_template.Wait)
                             .AddMinutes(_template.Required)).Seconds > 0)
                    {
                        newState = ContinentState.Dormant;
                        NextBoarding = NextBoarding.AddMinutes(_template.Term);
                    }

                    break;
            }

            if (newState == currentState) return;

            State = newState;
            Logger.Debug($"{_template.Info} continent state has been updated to {State}");

            switch (State)
            {
                case ContinentState.Dormant:
                    await MoveField(_template.MoveFieldID, _template.EndFieldID);
                    if (_template.CabinFieldID.HasValue)
                        await MoveField(_template.CabinFieldID.Value, _template.EndFieldID);
                    break;
                case ContinentState.Wait:
                    break;
                case ContinentState.Move:
                    await MoveField(_template.WaitFieldID, _template.MoveFieldID);
                    break;
            }
        }
    }
}