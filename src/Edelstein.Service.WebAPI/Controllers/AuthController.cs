using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Edelstein.Database.Entities;
using Edelstein.Service.WebAPI.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Edelstein.Service.WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private WebAPIService Service { get; }

        public AuthController(WebAPIService service)
        {
            Service = service;
        }

        private string GetToken(Account account)
        {
            var signingKey = Convert.FromBase64String(Service.Info.TokenKey);
            var expiryDuration = Service.Info.TokenExpiry;

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Issuer = null,
                Audience = null,
                IssuedAt = DateTime.UtcNow,
                NotBefore = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(expiryDuration),
                Subject = new ClaimsIdentity(new List<Claim>
                {
                    new Claim("account", account.ID.ToString())
                }),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(signingKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtTokenHandler = new JwtSecurityTokenHandler();
            var jwtToken = jwtTokenHandler.CreateJwtSecurityToken(tokenDescriptor);
            var token = jwtTokenHandler.WriteToken(jwtToken);

            return token;
        }

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login(LoginContract contract)
        {
            using (var store = Service.DataStore.OpenSession())
            {
                var account = store
                    .Query<Account>()
                    .FirstOrDefault(a => a.Username == contract.Username);

                if (account == null || !BCrypt.Net.BCrypt.Verify(contract.Password, account.Password))
                    return Unauthorized("Failed to authenticate");

                return Ok(GetToken(account));
            }
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(RegisterContract contract)
        {
            using (var store = Service.DataStore.OpenSession())
            {
                var account = store
                    .Query<Account>()
                    .FirstOrDefault(a => a.Username == contract.Username);

                if (account != null)
                    return Unauthorized("Account already exists");

                account = new Account
                {
                    Username = contract.Username,
                    Password = BCrypt.Net.BCrypt.HashPassword(contract.Password)
                };

                store.Insert(account);

                return Ok(GetToken(account));
            }
        }

        [Authorize]
        [HttpGet]
        [Route("test")]
        public string Test()
        {
            return "Accessed";
        }
    }
}