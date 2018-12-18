using System;

namespace Edelstein.Core.Services.Migrations
{
    [Serializable]
    public class MigrationInfo
    {
        public int ID { get; set; }
        public string FromService { get; set; }
        public string ToService { get; set; }
    }
}