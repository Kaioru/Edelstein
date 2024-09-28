using System;
using Edelstein.Protocol.Utilities;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Utilities.Tickers;

public abstract class AbstractTickerActionTerm(
    IDateTimeProvider dateTime,
    TimeSpan term
) : ITickerAction
{
    private DateTime DateLastUpdate { get; set; }
    
    public void Act()
    {
        var now = dateTime.Now;
        
        if (now - DateLastUpdate <= term) return;
        
        DateLastUpdate = now;
        ActAfter(now);
    }
    
    protected abstract void ActAfter(DateTime now);
}
