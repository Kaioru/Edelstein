using System;
using Edelstein.Core.Bootstrap.Providers.Scripting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Edelstein.Core.Bootstrap.Providers
{
    public class ConfiguredScriptingProvider : IProvider
    {
        private readonly string _section;

        public ConfiguredScriptingProvider(string section)
            => _section = section;

        public void Provide(HostBuilderContext context, IServiceCollection collection)
        {
            var section = context.Configuration.GetSection(_section);
            var type = Enum.Parse<ScriptingProviderType>(section["Type"]);

            switch (type)
            {
                case ScriptingProviderType.Python:
                    new PythonScriptingProvider(section["Path"]).Provide(context, collection);
                    break;
            }
        }
    }
}