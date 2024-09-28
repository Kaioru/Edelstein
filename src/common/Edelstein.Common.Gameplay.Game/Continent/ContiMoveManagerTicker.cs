using System;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game.Continent;

public class ContiMoveManagerTicker(
    IDateTimeProvider dateTime,
    IContiMoveManager manager
) : ITickerAction
{
    public void Act()
    {
        var now = dateTime.Now;
        var records = manager.RetrieveAll().Result;

        foreach (var contimove in records)
        {
            switch (contimove.State)
            {
                case ContiMoveState.Dormant:
                    if (now > contimove.NextBoarding)
                        contimove.Trigger(ContiMoveStateTrigger.Board);
                    break;
                case ContiMoveState.Wait:
                    if (now > contimove.NextStart)
                        contimove.Trigger(ContiMoveStateTrigger.Start);
                    break;
                case ContiMoveState.Move:
                    if (contimove.NextEvent.HasValue && now > contimove.NextEvent.Value)
                        contimove.Trigger(ContiMoveStateTrigger.MobGen);
                    break;
                case ContiMoveState.Event:
                    if (contimove.NextEventEnd.HasValue && now > contimove.NextEventEnd.Value)
                        contimove.Trigger(ContiMoveStateTrigger.MobDestroy);
                    break;
            }
        }
    }
}
