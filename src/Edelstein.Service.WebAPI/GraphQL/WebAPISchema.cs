using System;
using GraphQL.Types;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Service.WebAPI.GraphQL
{
    public class WebAPISchema : Schema
    {
        public WebAPISchema(IServiceProvider provider) : base(provider)
        {
            Query = provider.GetService<WebAPIQuery>();
            Mutation = provider.GetService<WebAPIMutation>();
            Subscription = provider.GetService<WebAPISubscription>();
        }
    }
}