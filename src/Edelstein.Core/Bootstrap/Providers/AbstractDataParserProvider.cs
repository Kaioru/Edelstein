namespace Edelstein.Core.Bootstrap.Providers
{
    public abstract class AbstractDataParserProvider : AbstractProvider
    {
        protected string Path { get; }
        
        protected AbstractDataParserProvider(string path)
        {
            Path = path;
        }
    }
}