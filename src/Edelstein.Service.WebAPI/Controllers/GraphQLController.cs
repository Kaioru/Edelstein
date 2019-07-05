using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Edelstein.Service.WebAPI.Contracts;
using Edelstein.Service.WebAPI.GraphQL;
using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Edelstein.Service.WebAPI.Controllers
{
    [ApiController]
    public class GraphQLController : Controller
    {
        private WebAPIService Service { get; }

        public GraphQLController(WebAPIService service)
        {
            Service = service;
        }

        [Authorize]
        [HttpPost]
        [Route("graphql")]
        public async Task<IActionResult> Post(GraphQLContract contract)
        {
            var accountID = Convert.ToInt32(((ClaimsIdentity) User.Identity).Claims
                .Single(c => c.Type == ClaimTypes.Sid).Value);
            var inputs = contract.Variables.ToInputs();
            var schema = new Schema
            {
                Query = new WebAPIQuery(Service, accountID)
            };
            var result = await new DocumentExecuter().ExecuteAsync(_ =>
            {
                _.Schema = schema;
                _.Query = contract.Query;
                _.OperationName = contract.OperationName;
                _.Inputs = inputs;
                _.UserContext = new WebAPIContext(Service);
                _.ThrowOnUnhandledException = true;
            });

            if (result.Errors?.Count > 0)
                return BadRequest(result);

            return Ok(result);
        }
    }
}