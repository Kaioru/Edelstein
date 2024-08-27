using System;
using Edelstein.Protocol.Utilities;

namespace Edelstein.Common.Utilities;

public class DateTimeProvider : IDateTimeProvider
{
    public DateTime Now => DateTime.UtcNow;
}
