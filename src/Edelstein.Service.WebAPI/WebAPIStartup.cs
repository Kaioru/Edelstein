using System;
using Edelstein.Service.WebAPI.GraphQL;
using Edelstein.Service.WebAPI.GraphQL.Context;
using GraphQL.Server;
using GraphQL.Server.Ui.Altair;
using GraphQL.Server.Ui.Playground;
using GraphQL.Server.Ui.Voyager;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Service.WebAPI
{
    public class WebAPIStartup
    {
        private readonly IWebHostEnvironment _environment;

        public WebAPIStartup(IWebHostEnvironment environment)
            => _environment = environment;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services
                .AddControllers()
                .AddNewtonsoftJson();

            services.AddSingleton<ISchema, WebAPISchema>();
            services
                .AddGraphQL()
                .AddGraphTypes()
                .AddGraphQLAuthorization(options =>
                {
                    options.AddPolicy("Authorized", p => p.RequireAuthenticatedUser());
                })
                .AddUserContextBuilder<WebAPIContextBuilder>()
                .AddDataLoader()
                .AddWebSockets()
                .AddNewtonsoftJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app)
        {
            if (_environment.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseGraphQL<ISchema>();
            app.UseGraphQLWebSockets<ISchema>();
            app.UseGraphQLPlayground(new GraphQLPlaygroundOptions());
            app.UseGraphQLVoyager(new GraphQLVoyagerOptions());
            app.UseGraphQLAltair(new GraphQLAltairOptions());
        }
    }
}