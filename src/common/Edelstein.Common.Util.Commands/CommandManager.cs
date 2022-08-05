using System.Collections.Immutable;
using System.Text.RegularExpressions;
using Edelstein.Protocol.Util.Commands;

namespace Edelstein.Common.Util.Commands;

public class CommandManager<TContext> : ICommandManager<TContext>
    where TContext : ICommandContext
{
    private readonly ICollection<ICommand<TContext>> _commands;
    private readonly Regex _regex = new("([\"'])(?:(?=(\\\\?))\\2.)*?\\1|([^\\s]+)", RegexOptions.Compiled);

    public CommandManager() =>
        _commands = new List<ICommand<TContext>>();

    public CommandManager(IEnumerable<ICommand<TContext>> commands) =>
        _commands = commands.ToList();

    public IReadOnlyCollection<ICommand<TContext>> Commands => _commands.ToImmutableList();

    public void Register(ICommand<TContext> command) =>
        _commands.Add(command);

    public void Deregister(ICommand<TContext> command) =>
        _commands.Remove(command);

    public virtual Task<bool> Process(TContext ctx, string text)
    {
        var args = _regex.Matches(text)
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
        return Process(ctx, args);
    }

    public virtual async Task<bool> Process(TContext ctx, string[] args)
    {
        if (args.Length <= 0) return false;

        var first = args[0];
        var command = GetCommand(first);

        switch (command)
        {
            case ICommandManager<TContext> manager:
                return await manager.Process(ctx, args[1..]);
            case null:
                return false;
            default:
                await command.Execute(ctx, args);
                return true;
        }
    }

    private ICommand<TContext>? GetCommand(string name) =>
        _commands.FirstOrDefault(c =>
            c.Name.StartsWith(name, StringComparison.OrdinalIgnoreCase) ||
            c.Aliases.Any(s => s.StartsWith(name, StringComparison.OrdinalIgnoreCase)));
}
