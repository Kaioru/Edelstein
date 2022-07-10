using Edelstein.Protocol.Parser;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Common.Parser.Duey
{
    public static class ServiceCollectionDueyExtensions
    {
        public static void AddNXParser(this IServiceCollection c, string directory)
        {
            c.AddSingleton<IDataDirectoryCollection>(new NXDataDirectoryCollection(directory));
        }
    }
}
