namespace Edelstein.Service.WebAPI
{
    public class WebAPIConfig
    {
        public string TokenKey { get; set; }
        public string TokenIssuer { get; set; }
        public string TokenAudience { get; set; }
        public int TokenExpiry { get; set; }
    }
}