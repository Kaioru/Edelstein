using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Models.Accounts;
using PowerArgs;

namespace Edelstein.Plugin.Rue.Commands;

public abstract class AbstractCommand<TArgs> : AbstractCommand where TArgs : CommandArgs
{
    public override async Task Execute(IFieldUser user, string[] args)
    {
        var def = new CommandLineArgumentsDefinition(typeof(TArgs)) { ExeName = Name };

        try
        {
            var parsed = Args.Parse<TArgs>(args);

            await Execute(user, parsed);
        }
        catch (ArgException e)
        {
            await user.Message(e.Message);
            await user.Message($"Usage: {def.UsageSummary}");
        }
    }

    protected abstract Task Execute(IFieldUser user, TArgs args);
}

public abstract class AbstractCommand : CommandManager, ICommand
{
    public string ID => Name;
    public abstract string Name { get; }
    public abstract string Description { get; }
    
    public ICollection<string> Aliases { get; }

    protected AbstractCommand()
        => Aliases = new List<string>();

    public override async Task<bool> Process(IFieldUser user, string[] args)
    {
        if (!await base.Process(user, args))
        {
            try
            {
                if (Check(user))
                    await Execute(user, args);
            }
            catch (Exception e)
            {
                await user.Message(e.Message);
            }
            return true;
        }

        return false;
    }

    public virtual bool Check(IFieldUser user) => true;
    public abstract Task Execute(IFieldUser user, string[] args);
}
