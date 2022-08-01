namespace Edelstein.Common.Gameplay.Database.Serializers;

public interface ISerializer
{
    byte[] Serialize<TObject>(TObject obj);
    TObject Deserialize<TObject>(byte[] data);
}
