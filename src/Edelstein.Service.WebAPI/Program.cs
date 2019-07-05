using Edelstein.Core.Bootstrap;

namespace Edelstein.Service.WebAPI
{
    public class Program
    {
        public static void Main(string[] args)
            => new Startup()
                .FromConfiguration(args)
                .ForService<WebAPIService, WebAPIInfo>()
                .StartAsync();
    }
}