using GraphQL;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPISchema : Schema
    {
        public WebAPISchema(IDependencyResolver resolver) : base(resolver)
        {
            Query = resolver.Resolve<WebAPIQuery>();
        }
    }
}