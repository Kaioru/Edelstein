namespace Edelstein.Common.Gameplay.Database.Transformers;

public interface ITransformer<TObject>
{
    TObject Transform(ITransformerContext ctx);
    ITransformerContext Transform(TObject obj);
}
