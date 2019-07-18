using Edelstein.Core.Distributed.Peers;

namespace Edelstein.Service.WebAPI
{
    public class WebAPIInfo : PeerServiceInfo
    {
        public string TokenKey { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public int TokenExpiry { get; set; }
    }
}