﻿using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Edelstein.Common.Utilities.Repositories;
using Edelstein.Protocol.Gameplay.Game.Objects.User;

namespace Edelstein.Plugin.Rue.Commands;

public partial class CommandManager : Repository<string, ICommand>, ICommandManager
{
    [GeneratedRegex("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)")]
    private static partial Regex CommandRegex();
    
    private async Task<ICommand?> GetCommand(IFieldUser user, string name)
    {
        var results = (await RetrieveAll())
            .Where(c => c.Check(user))
            .Where(c =>
                c.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) ||
                c.Aliases.Any(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase)))
            .ToImmutableArray();

        if (results.Length <= 1) return results.FirstOrDefault();
    
        var i = 0;
        var select = await user.Prompt(s => s.AskMenu($"Multiple command found with name '{name}', did you mean..", results.ToImmutableDictionary(
            c => i++,
            c => c.Name
        )), -1);

        return select == -1 
            ? null 
            : results[select];
    }
    
    public virtual Task<bool> Process(IFieldUser user, string text)
    {
        var regex = CommandRegex();
        var args = regex.Matches(text)
            .Select(m =>
            {
                var res = m.Value;

                if ((!res.StartsWith("'") || !res.EndsWith("'")) &&
                    (!res.StartsWith("\"") || !res.EndsWith("\"")))
                    return res;

                res = res[1..];
                res = res.Remove(res.Length - 1);
                return res;
            })
            .ToArray();
        return Process(user, args);
    }
    
    public virtual async Task<bool> Process(IFieldUser user, string[] args)
    {
        if (args.Length > 0)
        {
            var first = args[0];
            var command = await GetCommand(user, first);

            if (command != null) 
                return await command.Process(user, args[1..]);
        }

        return false;
    }
}
