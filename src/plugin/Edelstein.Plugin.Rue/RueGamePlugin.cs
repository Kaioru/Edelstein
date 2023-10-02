using System.Diagnostics;
using Edelstein.Plugin.Rue.Commands;
using Edelstein.Plugin.Rue.Commands.Admin;
using Edelstein.Plugin.Rue.Commands.Common;
using Edelstein.Plugin.Rue.Plugs;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Game;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Plugin.Rue;

public class RueGamePlugin : IGamePlugin
{
    public string ID => "RueGame";
    private ILogger? Logger { get; set; }
    
    public Task OnInit(IPluginHost<GameContext> host, GameContext ctx)
    {
        Logger = host.Logger;
        return Task.CompletedTask;
    }

    public async Task OnStart(IPluginHost<GameContext> host, GameContext ctx)
    {
        var commandManager = new CommandManager();

        await commandManager.Insert(new HelpCommand(commandManager));
        await commandManager.Insert(new AliasCommand(commandManager));

        await commandManager.Insert(new FieldCommand(
            ctx.Managers.Field,
            ctx.Templates.Field,
            ctx.Templates.FieldString
        ));
        await commandManager.Insert(new ItemCommand(
            ctx.Templates.Item,
            ctx.Templates.ItemString
        ));
        await commandManager.Insert(new SkillCommand(
            ctx.Templates.Skill,
            ctx.Templates.SkillString
        ));
        await commandManager.Insert(new QuestCommand(
            ctx.Templates.Quest
        ));
        await commandManager.Insert(new EquipCommand());
        await commandManager.Insert(new StatCommand());
        await commandManager.Insert(new TemporaryStatCommand());
        await commandManager.Insert(new MobTemporaryStatCommand());
        await commandManager.Insert(new DebugCommand());

        ctx.Pipelines.FieldOnPacketUserChat.Add(PipelinePriority.High, new FieldOnPacketUserChatCommandPlug(commandManager));

        _ = Task.Run(async () =>
        {
            foreach (var command in (await commandManager.RetrieveAll()).OfType<IIndexedCommand>())
            {
                var stopwatch = new Stopwatch();
                
                stopwatch.Start();
                await command.Index();
                
                host.Logger.LogDebug(
                    "Finished indexing for command {Command} in {Elapsed:F2}ms", 
                    command.Name, 
                    stopwatch.Elapsed.TotalMilliseconds
                );
            }
        });
    }

    public Task OnStop()
        => Task.CompletedTask;
}
