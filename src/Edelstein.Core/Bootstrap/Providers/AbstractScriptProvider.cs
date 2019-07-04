namespace Edelstein.Core.Bootstrap.Providers
{
    public abstract class AbstractScriptProvider : AbstractProvider
    {
        protected string Path { get; }
        
        protected AbstractScriptProvider(string path)
        {
            Path = path;
        }
    }
}