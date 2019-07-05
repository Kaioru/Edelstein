using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Edelstein.Service.WebAPI.Contracts;
using Edelstein.Service.WebAPI.GraphQL;
using Edelstein.Service.WebAPI.GraphQL.Types;
using GraphQL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace Edelstein.Service.WebAPI.Controllers
{
    [ApiController]
    public class GraphQLController : Controller
    {
        private WebAPIService Service { get; }
        private IServiceProvider Provider { get; }

        public GraphQLController(WebAPIService service, IServiceProvider provider)
        {
            Service = service;
            Provider = provider;
        }

        [Authorize]
        [HttpPost]
        [Route("graphql")]
        public async Task<IActionResult> Post(GraphQLContract contract)
        {
            var inputs = contract.Variables.ToInputs();
            var schema = Provider.GetService<WebAPISchema>();
            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = contract.Query;
                _.OperationName = contract.OperationName;
                _.Inputs = inputs;
                _.UserContext = User.Identity;
            });

            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result);
        }
    }
}