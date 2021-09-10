using Edelstein.Protocol.Services.Contracts;
using GraphQL.Types;

namespace Edelstein.App.WebAPI.GraphQL.Types
{
    public class ServerType : ObjectGraphType<ServerContract>
    {
        public ServerType()
        {
            Field("id", x => x.Id);

            Field("host", x => x.Host);
            Field("port", x => x.Port);

            Field<ListGraphType<ServerMetadataType>>("metadata", resolve: ctx => ctx.Source.Metadata);
        }
    }
}
