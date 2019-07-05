using System.Collections.Generic;
using Edelstein.Database.Entities.Characters;

namespace Edelstein.Service.WebAPI.GraphQL.Models
{
    public class AccountDataModel
    {
        public byte WorldID { get; set; }
        public ICollection<Character> Characters { get; set; }
    }
}