using Edelstein.App.WebAPI.GraphQL;
using Edelstein.Common.Services;
using Edelstein.Protocol.Services;
using GraphQL.Server;
using GraphQL.Types;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.App.WebAPI
{
    public class ProgramStartup
    {
        public static void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IServerRegistry, ServerRegistry>();

            services.AddControllers();

            services.AddSingleton<ISchema, WebAPISchema>();
            services
                .AddGraphQL()
                .AddGraphTypes()
                .AddErrorInfoProvider(opt => opt.ExposeExceptionStackTrace = true)
                .AddGraphQLAuthorization(options =>
                {
                    options.AddPolicy("Authorized", p => p.RequireAuthenticatedUser());
                })
                .AddDataLoader()
                .AddSystemTextJson();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public static void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
                app.UseDeveloperExceptionPage();

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints => { endpoints.MapControllers(); });

            app.UseGraphQL<ISchema>();
            app.UseGraphQLPlayground();
            app.UseGraphQLVoyager();
            app.UseGraphQLAltair();
        }
    }
}
