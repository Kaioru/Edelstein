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
    public string ID => "Rue";
    private ILogger? Logger { get; set; }
    
    public Task OnInit(IPluginHost<GameContext> host, GameContext ctx)
    {
        Logger = host.Logger;
        return Task.CompletedTask;
    }

    public Task OnStart(IPluginHost<GameContext> host, GameContext ctx)
    {
        var commandManager = new CommandManager();

        commandManager.Insert(new HelpCommand(commandManager));
        commandManager.Insert(new AliasCommand(commandManager));

        commandManager.Insert(new FieldCommand(
            ctx.Managers.Field,
            ctx.Templates.Field,
            ctx.Templates.FieldString
        ));
        commandManager.Insert(new ItemCommand(
            ctx.Templates.Item,
            ctx.Templates.ItemString
        ));
        commandManager.Insert(new SkillCommand(
            ctx.Templates.Skill,
            ctx.Templates.SkillString
        ));
        commandManager.Insert(new QuestCommand(
            ctx.Templates.Quest
        ));
        commandManager.Insert(new EquipCommand());
        commandManager.Insert(new StatCommand());
        commandManager.Insert(new TemporaryStatCommand());
        commandManager.Insert(new MobTemporaryStatCommand());
        
        commandManager.Insert(new DebugCommand());

        ctx.Pipelines.FieldOnPacketUserChat.Add(PipelinePriority.High, new FieldOnPacketUserChatCommandPlug(commandManager));
        return Task.CompletedTask;
    }
    
    public Task OnStop()
    {
        return Task.CompletedTask;
    }
}
