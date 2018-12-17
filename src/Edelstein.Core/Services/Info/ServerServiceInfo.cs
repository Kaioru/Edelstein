using System;

namespace Edelstein.Core.Services.Info
{
    [Serializable]
    public class ServerServiceInfo : ServiceInfo
    {
        public string Host { get; set; }
        public int Port { get; set; }
    }
}