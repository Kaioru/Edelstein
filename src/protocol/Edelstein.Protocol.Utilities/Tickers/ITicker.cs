using System;
using System.Threading.Tasks;

namespace Edelstein.Protocol.Utilities.Tickers;

public interface ITicker : IDisposable
{
    Task Register(ITickerAction action);
}
