using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Edelstein.Protocol.Plugin;

namespace Edelstein.Common.Plugin;

public record PluginHost<TContext>(
    IPluginHostManifest? Manifest,
    IPluginManager<TContext> Manager,
    IPlugin<TContext> Plugin
) : IPluginHost<TContext>
{
    private readonly Dictionary<string, List<MethodInfo>> _exports = new();

    public string ID => Plugin.ID;
    
    public void Export<T>()
    {
        foreach (var method in typeof(T)
                     .GetMethods(BindingFlags.Public | BindingFlags.Static))
        {
            if (!_exports.TryGetValue(method.Name, out var methods))
                _exports[method.Name] = methods = new List<MethodInfo>();
        
            if (!methods.Contains(method))
                methods.Add(method);
        }
    }
    
    public void Import<T>()
    {
        foreach (var field in typeof(T)
                     .GetFields(BindingFlags.Public | BindingFlags.Static)
                     .Where(f => typeof(Delegate).IsAssignableFrom(f.FieldType)))
        {
            if (!_exports.TryGetValue(field.Name, out var methods))
            {
                field.SetValue(null, null);
                continue;
            }

            foreach (var method in methods)
            {
                try
                {
                    field.SetValue(null, Delegate.CreateDelegate(field.FieldType, null, method));
                    break;
                }
                catch
                {
                    continue;
                }
            }
        }
    }
}
