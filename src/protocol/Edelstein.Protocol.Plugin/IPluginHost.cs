﻿using Edelstein.Protocol.Utilities.Repositories;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Edelstein.Protocol.Plugin;

public interface IPluginHost<TContext> : IIdentifiable<string>
{
    IPluginHostManifest? Manifest { get; }
    
    ILogger Logger { get; }
    IConfiguration Config { get; }

    string DirectoryHost { get; }
    string DirectoryPlugin { get; }

    IPluginManager<TContext> Manager { get; }
    IPlugin<TContext> Plugin { get; }
}
