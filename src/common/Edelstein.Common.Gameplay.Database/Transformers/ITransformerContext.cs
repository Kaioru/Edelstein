namespace Edelstein.Common.Gameplay.Database.Transformers;

public interface ITransformerContext
{
    int Version { get; }
    byte[] Bytes { get; }
}
