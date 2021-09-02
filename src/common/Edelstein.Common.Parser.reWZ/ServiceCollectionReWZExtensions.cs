using Edelstein.Protocol.Parser;
using Microsoft.Extensions.DependencyInjection;
using reWZ;

namespace Edelstein.Common.Parser.reWZ
{
    public static class ServiceCollectionReWZExtensions
    {
        public static void AddWZParser(this IServiceCollection c, string directory, WZVariant variant, bool encrypted)
        {
            c.AddSingleton<IDataDirectoryCollection>(new WZDataDirectoryCollection(directory, variant, encrypted));
        }
    }
}

