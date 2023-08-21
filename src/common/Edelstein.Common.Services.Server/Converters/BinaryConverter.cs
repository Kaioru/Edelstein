using Ceras;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Edelstein.Common.Services.Server.Converters;

public class BinaryConverter<T> : ValueConverter<T, byte[]>
{
    public BinaryConverter(CerasSerializer serializer) : base(
        v => serializer.Serialize(v),
        s => serializer.Deserialize<T>(s)
    )
    {
    }
}
