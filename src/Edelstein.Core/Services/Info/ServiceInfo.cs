using System;

namespace Edelstein.Core.Services.Info
{
    [Serializable]
    public class ServiceInfo
    {
        public byte ID { get; set; }
        public string Name { get; set; }
    }
}