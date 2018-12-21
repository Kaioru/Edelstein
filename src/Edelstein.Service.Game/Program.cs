using Autofac;
using Edelstein.Core.Services;
using Edelstein.Core.Services.Startup;
using Edelstein.Service.Game.Conversations;
using Edelstein.Service.Game.Conversations.Messages;
using Edelstein.Service.Game.Conversations.Scripts;
using Edelstein.Service.Game.Conversations.Scripts.Lua;
using Microsoft.Extensions.Configuration;
using MoonSharp.Interpreter;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace Edelstein.Service.Game
{
    public static class Program
    {
        private static void Main(string[] args)
            => ServiceBootstrap<WvsGame>.Build()
                .WithLogging(new LoggerConfiguration()
                    .MinimumLevel.Verbose()
                    .WriteTo.Console(theme: AnsiConsoleTheme.Code)
                    .CreateLogger())
                .WithConfig("WvsGame", new WvsGameOptions())
                .WithDistributed()
                .WithMySQLDatabase()
                .WithNXProvider()
                .WithLuaScripts()
                .Run()
                .Wait();

        public static ServiceBootstrap<T> WithLuaScripts<T>(this ServiceBootstrap<T> bootstrap) where T : IService
        {
            bootstrap.Builder.Register(c =>
            {
                var path = c.Resolve<IConfigurationRoot>()["ScriptDirectoryPath"];

                UserData.RegisterType<Speaker>();
                UserData.RegisterType<QuizSpeaker>();
                UserData.RegisterType<SpeedQuizSpeaker>();
                return new LuaScriptConversationManager(path);
            }).As<IScriptConversationManager>();
            return bootstrap;
        }
    }
}