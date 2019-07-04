using Edelstein.Database.Store;
using Microsoft.AspNetCore.Mvc;

namespace Edelstein.Service.WebAPI.Controllers
{
    [ApiController]
    [Route("auth")]
    public class AuthController : Controller
    {
        private readonly IDataStore _dataStore;

        public AuthController(IDataStore dataStore)
        {
            _dataStore = dataStore;
        }

        [HttpGet]
        [Route("hello")]
        public string Hello()
        {
            return "hello world";
        }
    }
}