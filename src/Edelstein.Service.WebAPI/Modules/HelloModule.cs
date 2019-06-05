using Nancy;

namespace Edelstein.Service.WebAPI.Modules
{
    public sealed class HelloModule : NancyModule
    {
        public HelloModule()
        {
            Get("/hello", parameters => "Hello World!");
        }
    }
}