using System;
using Edelstein.Common.Utilities.Tickers;
using Edelstein.Protocol.Gameplay.Game.Continents;
using Edelstein.Protocol.Utilities;

namespace Edelstein.Common.Gameplay.Game.Continent;

public class ContiMoveManagerTicker(
    IDateTimeProvider dateTime,
    IContiMoveManager manager
) : AbstractTickerActionTerm(dateTime, TimeSpan.FromSeconds(20))
{
    protected override void ActAfter(DateTime now)
    {
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
                    if (now > contimove.NextEnd)
                        contimove.Trigger(ContiMoveStateTrigger.End);
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
