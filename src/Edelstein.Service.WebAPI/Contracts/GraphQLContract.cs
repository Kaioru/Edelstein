using Newtonsoft.Json.Linq;

namespace Edelstein.Service.WebAPI.Contracts
{
    public class GraphQLContract
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }
    }
}