using Edelstein.Database;

namespace Edelstein.Entities
{
    public class AccountWorld : IDataEntity
    {
        public int ID { get; set; }
        
        public int AccountID { get; set; }
        public byte WorldID { get; set; }
    }
}