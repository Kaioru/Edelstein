using Edelstein.Core.Distributed;
using GraphQL.Types;

namespace Edelstein.Service.WebAPI.GraphQL.Types
{
    public sealed class ServerStateType : ObjectGraphType<IServerNodeState>
    {
        public ServerStateType(WebAPIService service)
        {
            Field<string>("name", x => x.Name);
            Field<string>("scope", x => x.Scope);

            Field<string>("host", x => x.Host);
            Field<int>("port", x => x.Port);

            Field<int>("version", x => x.Version);
            Field<string>("patch", x => x.Patch);
            Field<int>("locale", x => x.Locale);

            Field<IntGraphType>("userNo", resolve: ctx =>
                service.SocketCountCache.GetAsync<int>(ctx.Source.Name).Result.Value
            );
        }
    }
}