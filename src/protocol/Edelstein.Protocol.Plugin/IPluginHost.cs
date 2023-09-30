using Edelstein.Protocol.Utilities.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext> : IIdentifiable<string>
{
    ILogger Logger { get; }
    IConfiguration Config { get; }
    
    IPlugin<TContext> Plugin { get; }
}
