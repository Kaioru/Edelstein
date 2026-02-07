using Edelstein.Plugin.Rue.Commands;
using Edelstein.Plugin.Rue.Commands.Admin;
using Edelstein.Plugin.Rue.Commands.Common;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Plugin.Rue.Plugs;
using Edelstein.Protocol.Gameplay.Game.Contexts;
using Edelstein.Protocol.Plugin;
using Edelstein.Protocol.Plugin.Game;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue;

/// <summary>
/// Game server plugin providing admin commands and template search functionality.
/// </summary>
public class RueGamePlugin : IGamePlugin
{
    public string ID => "RueGame";

    private IOptions<RueConfigGame> _options = Options.Create(new RueConfigGame());

    public Task OnInit(IPluginHost<GameContext> host, GameContext ctx)
    {
        return Task.CompletedTask;
    }

    public async Task OnStart(IPluginHost<GameContext> host, GameContext ctx)
    {
        _options = RueConfigBinding.BindGameOptions(host.Config);

        var commandManager = new CommandManager();

        await RegisterCommands(commandManager, host, ctx);

        ctx.Pipelines.FieldOnPacketUserChat.Add(
            PipelinePriority.High,
            new FieldOnPacketUserChatCommandPlug(commandManager));

        _ = RunIndexingAsync(commandManager, host.Logger, _options);
    }

    public Task OnStop()
        => Task.CompletedTask;

    private static async Task RegisterCommands(
        CommandManager commandManager,
        IPluginHost<GameContext> host,
        GameContext ctx)
    {
        // Core commands
        await commandManager.Insert(new HelpCommand(commandManager));
        await commandManager.Insert(new AliasCommand(commandManager));

        // Template-based commands (indexed)
        await commandManager.Insert(new FieldCommand(
            ctx.Managers.Field,
            ctx.Templates.Field,
            ctx.Templates.FieldString));

        await commandManager.Insert(new NPCCommand(
            ctx.Templates.NPC,
            ctx.Templates.NPCString));

        await commandManager.Insert(new MobCommand(
            ctx.Templates.Mob,
            ctx.Templates.MobString));

        await commandManager.Insert(new ItemCommand(
            ctx.Templates.Item,
            ctx.Templates.ItemString));

        await commandManager.Insert(new SkillCommand(
            ctx.Templates.Skill,
            ctx.Templates.SkillString));

        await commandManager.Insert(new QuestCommand(
            ctx.Templates.Quest));

        // Utility commands
        await commandManager.Insert(new MobTemporaryStatCommand());
        await commandManager.Insert(new ContiMoveCommand(ctx.Managers.ContiMove));
        await commandManager.Insert(new EquipCommand());
        await commandManager.Insert(new StatCommand());
        await commandManager.Insert(new TemporaryStatCommand());
        await commandManager.Insert(new ClearDropsCommand());
        await commandManager.Insert(new DropCommand(ctx.Managers.MobRewardPool));
        await commandManager.Insert(new RateCommand(ctx.Managers.Rates));

        // Admin/debug commands
        await commandManager.Insert(new PluginCommand(host.Manager));
        await commandManager.Insert(new DebugCommand());
    }

    private static async Task RunIndexingAsync(CommandManager commandManager, ILogger logger, IOptions<RueConfigGame> options)
    {
        try
        {
            var indexedCommands = (await commandManager.RetrieveAll())
                .OfType<IIndexedCommand>()
                .ToList();

            if (indexedCommands.Count == 0)
            {
                logger.LogDebug("No indexed commands found, skipping indexing");
                return;
            }

            var indexingStatus = new IndexingStatus();

            foreach (var command in indexedCommands)
                indexingStatus.Register(command.Name);

            logger.LogInformation(
                "Starting parallel indexing for {Count} commands: {Commands}",
                indexedCommands.Count,
                string.Join(", ", indexedCommands.Select(static c => c.Name)));

            var totalStartTick = Environment.TickCount64;
            var logTrieTelemetry = options.Value.LogTrieTelemetry;

            await Parallel.ForEachAsync(
                indexedCommands,
                new ParallelOptions { MaxDegreeOfParallelism = Environment.ProcessorCount },
                async (command, ct) =>
                {
                    try
                    {
                        var commandStartTick = Environment.TickCount64;
                        await command.Index(indexingStatus);

                        if (logTrieTelemetry && indexingStatus.TryGetTrieTelemetry(command.Name, out var telemetry))
                        {
                            logger.LogInformation(
                                "Trie telemetry {Command}: enabled={Enabled} total={TotalIndices} added={AddedKeys} desc={DescriptionIndices} null={NormalizedNull} short={NormalizedTooShort} long={NormalizedTooLong} dup={DuplicateKeys} addArgEx={AddArgumentExceptions} addOorEx={AddOutOfRangeExceptions} addNreEx={AddNullReferenceExceptions} retrieveEx={RetrieveExceptions} buildMs={BuildElapsedMs}",
                                telemetry.CommandName,
                                telemetry.TrieEnabled,
                                telemetry.TotalIndices,
                                telemetry.AddedKeys,
                                telemetry.DescriptionIndices,
                                telemetry.NormalizedNull,
                                telemetry.NormalizedTooShort,
                                telemetry.NormalizedTooLong,
                                telemetry.DuplicateKeys,
                                telemetry.AddArgumentExceptions,
                                telemetry.AddOutOfRangeExceptions,
                                telemetry.AddNullReferenceExceptions,
                                telemetry.RetrieveExceptions,
                                telemetry.BuildElapsedMs);
                        }

                        logger.LogDebug(
                            "Finished indexing for command {Command} in {Elapsed}ms",
                            command.Name,
                            Environment.TickCount64 - commandStartTick);
                    }
                    catch (Exception ex)
                    {
                        logger.LogError(ex,
                            "Failed to index command {Command}",
                            command.Name);
                    }
                });

            logger.LogInformation(
                "Indexing completed for all {Count} commands in {Elapsed}ms (parallel)",
                indexedCommands.Count,
                Environment.TickCount64 - totalStartTick);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Fatal error during command indexing");
        }
    }
}
