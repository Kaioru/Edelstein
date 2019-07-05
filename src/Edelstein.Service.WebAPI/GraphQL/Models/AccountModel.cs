using System.Collections.Generic;

namespace Edelstein.Service.WebAPI.GraphQL.Models
{
    public class AccountModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public ICollection<AccountDataModel> Data { get; set; }
    }
}