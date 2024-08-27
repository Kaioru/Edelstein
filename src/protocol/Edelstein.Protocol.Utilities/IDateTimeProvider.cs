using System;

namespace Edelstein.Protocol.Utilities;

public interface IDateTimeProvider
{
    DateTime Now { get; }
}
