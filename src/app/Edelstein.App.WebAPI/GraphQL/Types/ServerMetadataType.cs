using System.Collections.Generic;
using GraphQL.Types;

namespace Edelstein.App.WebAPI.GraphQL.Types
{
    public class ServerMetadataType : ObjectGraphType<KeyValuePair<string, string>>
    {
        public ServerMetadataType()
        {
            Field("key", x => x.Key);
            Field("value", x => x.Value);
        }
    }
}
