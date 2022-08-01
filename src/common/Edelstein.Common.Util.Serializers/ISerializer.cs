namespace Edelstein.Common.Util.Serializers;

public interface ISerializer
{
    byte[] Serialize<TObject>(TObject obj);
    TObject Deserialize<TObject>(byte[] data);
}
