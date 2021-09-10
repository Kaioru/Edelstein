using Edelstein.App.WebAPI.GraphQL.Types;
using Edelstein.Protocol.Services;
using Edelstein.Protocol.Services.Contracts;
using GraphQL.Types;

namespace Edelstein.App.WebAPI.GraphQL
{
    public class WebAPIQuery : ObjectGraphType
    {
        public WebAPIQuery(IServerRegistry serverRegistry)
        {
            Field<ListGraphType<ServerType>>()
                .Name("servers")
                .ResolveAsync(async ctx =>
                {
                    var request = new DescribeServerByMetadataRequest();
                    var response = await serverRegistry.DescribeByMetadata(request);

                    return response.Servers;
                });
        }
    }
}
