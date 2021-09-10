using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.App.WebAPI.GraphQL
{
    public class WebAPISchema : Schema
    {
        public WebAPISchema(IServiceProvider services) : base(services)
        {
            Query = services.GetRequiredService<WebAPIQuery>();
            Mutation = services.GetRequiredService<WebAPIMutation>();
        }
    }
}
