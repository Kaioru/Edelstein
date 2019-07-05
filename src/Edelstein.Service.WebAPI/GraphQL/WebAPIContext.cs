namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPIContext
    {
        public WebAPIService Service { get; }

        public WebAPIContext(WebAPIService service)
        {
            Service = service;
        }
    }
}