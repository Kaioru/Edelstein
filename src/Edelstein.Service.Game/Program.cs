using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Info;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Quiz;
using Edelstein.Service.Game.Conversations.Scripts;
using Edelstein.Service.Game.Conversations.Scripts.Lua;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using MoonSharp.Interpreter;

namespace Edelstein.Service.Game
{
    public static class Program
    {
        private static Task Main(string[] args)
            => new Startup()
                .WithConfig()
                .WithLogger()
                .WithInferredModel()
                .WithInferredDatabase()
                .WithInferredProvider()
                .WithInferredScripting()
                .WithServiceOption<GameServiceInfo>()
                .WithService<WvsGame>()
                .Start();

        public static Startup WithInferredScripting(this Startup startup)
        {
            startup.Builder.ConfigureServices((context, services) =>
            {
                switch (context.Configuration["scripting"].ToLower())
                {
                    case "lua":
                    default:
                        WithLuaScripting(context, services);
                        break;
                }
            });
            return startup;
        }

        private static void WithLuaScripting(HostBuilderContext context, IServiceCollection services)
        {
            UserData.RegisterType<Speaker>();
            UserData.RegisterType<QuizSpeaker>();
            UserData.RegisterType<SpeedQuizSpeaker>();

            UserData.RegisterType<FieldSpeaker>();
            UserData.RegisterType<FieldUserSpeaker>();
            UserData.RegisterType<FieldNPCSpeaker>();

            services.AddSingleton<IScriptConversationManager>(
                new LuaScriptConversationManager(context.Configuration["scriptPath"])
            );
        }
    }
}