using Ceras;

namespace Edelstein.Common.Gameplay.Database.Serializers;

public class BinarySerializer : ISerializer
{
    private readonly CerasSerializer _serializer;

    public BinarySerializer() => _serializer = new CerasSerializer();

    public byte[] Serialize<TObject>(TObject obj) =>
        _serializer.Serialize(obj);

    public TObject Deserialize<TObject>(byte[] data) =>
        _serializer.Deserialize<TObject>(data);
}
