using System.Collections.Immutable;
using Edelstein.Protocol.Util.Commands;
using PowerArgs;

namespace Edelstein.Common.Util.Commands;

public abstract class AbstractCommand<TContext, TArgs> : AbstractCommand<TContext> where TArgs : CommandArgs
{
    public override async Task Execute(TContext ctx, string[] args)
    {
        var def = new CommandLineArgumentsDefinition(typeof(TArgs)) { ExeName = Name };

        try
        {
            var parsed = Args.Parse<TArgs>(args);

            await Execute(ctx, parsed);
        }
        catch (ArgException e)
        {
            //await user.Message(e.Message);
            //await user.Message($"Usage: {def.UsageSummary}");
        }
    }

    protected abstract Task Execute(TContext ctx, TArgs args);
}

public abstract class AbstractCommand<TContext> : CommandManager<TContext>, ICommand<TContext>
{
    private readonly ICollection<string> _aliases;

    protected AbstractCommand()
        => _aliases = new HashSet<string>();

    public abstract string Name { get; }
    public abstract string Description { get; }

    public IEnumerable<string> Aliases => _aliases.ToImmutableList();

    public void AddAlias(string alias) =>
        _aliases.Add(alias);

    public void RemoveAlias(string alias) =>
        _aliases.Remove(alias);

    public override async Task<bool> Process(TContext ctx, string[] args)
    {
        if (await base.Process(ctx, args)) return false;

        try
        {
            await Execute(ctx, args);
        }
        catch (Exception)
        {
            // TODO: error handling
        }

        return true;
    }

    public abstract Task Execute(TContext ctx, string[] args);
}
