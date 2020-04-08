using System.Collections.Generic;

namespace Edelstein.Service.WebAPI.GraphQL.Context
{
    public class WebAPIContext : Dictionary<string, object>
    {
        public int AccountID { get; set; }
    }
}