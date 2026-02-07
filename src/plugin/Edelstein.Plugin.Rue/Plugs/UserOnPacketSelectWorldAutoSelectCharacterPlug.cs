using System.Collections.Immutable;
using Edelstein.Plugin.Rue.ClientAnalysis;
using Edelstein.Plugin.Rue.Configs;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contexts;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Login.Types;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Utilities.Pipelines;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Edelstein.Plugin.Rue.Plugs;

/// <summary>
/// Automatically selects character and enters game after world selection when auto-login is enabled.
/// Supports auto-creation of characters when none exist.
/// </summary>
public class UserOnPacketSelectWorldAutoSelectCharacterPlug(
    ILogger? logger,
    IOptions<RueConfigLogin> options,
    LoginContext context,
    ICharacterRepository characterRepository,
    MemoryContext? memoryContext = null,
    LoginMilestonesTracker? tracker = null) : IPipelinePlug<UserOnPacketSelectWorld>
{
    private readonly ILogger? _logger = logger;
    private readonly RueConfigLogin _config = options.Value;
    private readonly LoginContext _context = context;
    private readonly ICharacterRepository _characterRepository = characterRepository;
    private readonly MemoryContext? _memoryContext = memoryContext;
    private readonly LoginMilestonesTracker? _tracker = tracker;

    private LoginStepMonitor? Monitor => _memoryContext?.Monitor;

    public async Task Handle(IPipelineContext ctx, UserOnPacketSelectWorld message)
    {
        if (ctx.IsRequestedCancellation)
            return;

        if (!_config.IsAutoLogin)
            return;

        // In passive mode, skip auto-character selection
        var watchMode = _config.ClientMemory?.WatchMode ?? RueConfigClientMemory.WatchModeActive;
        if (watchMode.Equals(RueConfigClientMemory.WatchModePassive, StringComparison.OrdinalIgnoreCase))
        {
            _logger?.LogInformation("[Rue-AutoLogin] Passive mode - skipping auto-character selection");
            return;
        }

        // Only proceed if world selection was successful and state transitioned to SelectCharacter
        if (message.User.State != LoginState.SelectCharacter)
            return;

        _tracker?.RecordServerState(message.User.State);

        if (message.User.AccountWorld == null)
            return;

        var t0 = Environment.TickCount64;

        // Get the character list
        var characters = (await _characterRepository.RetrieveAllByAccountWorld(message.User.AccountWorld.ID))
            .ToImmutableArray();

        ICharacter? selectedCharacter = null;

        // If no characters exist and auto-create is enabled, create one
        if (characters.Length == 0)
        {
            if (_config.AutoCreateCharacter && _config.AutoCharacterConfig != null)
            {
                selectedCharacter = await CreateAutoCharacter(message.User);
            }

            if (selectedCharacter == null)
            {
                _logger?.LogWarning("[Rue-AutoLogin] No characters and auto-create disabled for {Username}",
                    message.User.Account?.Username);
                return;
            }
        }
        else
        {
            // Find character by name or index
            selectedCharacter = FindCharacter(characters);
        }

        if (selectedCharacter == null)
        {
            _logger?.LogWarning("[Rue-AutoLogin] No matching character for {Username}",
                message.User.Account?.Username);
            return;
        }

        // Wait for client to finish step transition (fade animation) before selecting character
        if (Monitor != null)
        {
            var stableOk = await Monitor.WaitWithTimeout(
                ct => Monitor.WaitForStepTransitionComplete(ct),
                "StepTransitionComplete before character select");

            if (!stableOk)
                _logger?.LogWarning("[Rue-AutoLogin] Step transition timeout before character select - proceeding");
        }
        else
        {
            var delay = _config.AutoSelectDelayMs;
            if (delay > 0)
                await Task.Delay(delay);
        }

        var transitionElapsed = Environment.TickCount64 - t0;
        _logger?.LogInformation("[Rue-AutoLogin] Step transition complete (+{Elapsed}), selecting character {Name} (ID:{ID})",
            LoginMilestonesTracker.FormatElapsed(transitionElapsed), selectedCharacter.Name, selectedCharacter.ID);

        // Determine if we need to enable SPW or check existing SPW
        var hasSPW = !string.IsNullOrEmpty(message.User.Account?.SPW);
        var autoSPW = _config.AutoSPW ?? "0000"; // Default SPW if not configured

        if (hasSPW)
        {
            // Account has SPW, use CheckSPWRequest
            await _context.Pipelines.UserOnPacketCheckSPWRequest.Process(new UserOnPacketCheckSPWRequest(
                message.User,
                autoSPW,
                selectedCharacter.ID,
                "00-00-00-00-00-00",
                "00-00-00-00-00-00_00000000"
            ));
        }
        else
        {
            // Account doesn't have SPW, use EnableSPWRequest to set it and enter game
            await _context.Pipelines.UserOnPacketEnableSPWRequest.Process(new UserOnPacketEnableSPWRequest(
                message.User,
                selectedCharacter.ID,
                "00-00-00-00-00-00",
                "00-00-00-00-00-00_00000000",
                autoSPW
            ));
        }

        _logger?.LogInformation("[Rue-AutoLogin] Character selected (+{Elapsed})", LoginMilestonesTracker.FormatElapsed(Environment.TickCount64 - t0));
        _tracker?.RecordCompletion();
    }


    private ICharacter? FindCharacter(ImmutableArray<ICharacter> characters)
    {
        // Try to find by name first
        if (!string.IsNullOrEmpty(_config.AutoSelectCharacterName))
        {
            var byName = characters.FirstOrDefault(c =>
                c.Name.Equals(_config.AutoSelectCharacterName, StringComparison.OrdinalIgnoreCase));
            if (byName != null)
                return byName;
        }

        // Then try by index
        if (_config.AutoSelectCharacterIndex != null)
        {
            var index = _config.AutoSelectCharacterIndex.Value;
            if (index >= 0 && index < characters.Length)
                return characters[index];
        }

        // Default to first character
        return characters.FirstOrDefault();
    }

    private async Task<ICharacter?> CreateAutoCharacter(ILoginStageUser user)
    {
        var config = _config.AutoCharacterConfig!;
        var baseName = config.NamePrefix;
        var name = baseName;
        var suffix = 1;

        // Find a unique name
        while (await _characterRepository.CheckExistsByName(name))
        {
            name = $"{baseName}{suffix}";
            suffix++;

            if (suffix > 9999) // Safety limit
            {
                _logger?.LogError("[Rue-AutoLogin] No unique name available with prefix {Prefix}", baseName);
                return null;
            }
        }

        _logger?.LogInformation("[Rue-AutoLogin] Creating character {Name}", name);

        // Trigger character creation pipeline
        var createResult = await _context.Pipelines.UserOnPacketCreateNewCharacter.Process(
            new UserOnPacketCreateNewCharacter(
                user,
                name,
                (RaceSelectType)config.Race,
                config.SubJob,
                config.Face,
                config.Hair,
                config.HairColor,
                config.Skin,
                config.Coat,
                config.Pants,
                config.Shoes,
                config.Weapon,
                config.Gender
            ));

        if (createResult.IsRequestedCancellation)
        {
            _logger?.LogWarning("[Rue-AutoLogin] Character creation cancelled");
            return null;
        }

        // Retrieve the newly created character
        return await _characterRepository.RetrieveByName(name);
    }
}
