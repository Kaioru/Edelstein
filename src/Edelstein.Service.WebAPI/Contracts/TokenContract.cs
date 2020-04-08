using System;

namespace Edelstein.Service.WebAPI.Contracts
{
    public class TokenContract
    {
        public string Token { get; set; }
        public DateTime Expire { get; set; }
    }
}