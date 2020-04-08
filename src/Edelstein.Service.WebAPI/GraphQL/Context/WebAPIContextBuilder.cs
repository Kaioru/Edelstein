using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore;
using Microsoft.AspNetCore.Http;

namespace Edelstein.Service.WebAPI.GraphQL.Context
{
    public class WebAPIContextBuilder : IUserContextBuilder
    {
        public Task<IDictionary<string, object>> BuildUserContext(HttpContext httpContext)
        {
            return Task.FromResult<IDictionary<string, object>>(new WebAPIContext
            {
                AccountID = Convert.ToInt32(
                    httpContext.User.Claims.SingleOrDefault(c => c.Type == ClaimTypes.Name)?.Value
                )
            });
        }
    }
}