using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts.Pipelines;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CharacterCreatePlug : IPipelinePlug<ICharacterCreate>
{
    private readonly ILogger _logger;
    private readonly ICharacterRepository _characters;
    private readonly ITemplateManager<IItemTemplate> _items;

    public CharacterCreatePlug(
        ILogger<CharacterCreatePlug> logger,
        ICharacterRepository characters,
        ITemplateManager<IItemTemplate> items
    )
    {
        _logger = logger;
        _characters = characters;
        _items = items;
    }

    public async Task Handle(IPipelineContext ctx, ICharacterCreate message)
    {
        var face = message.Look[0];
        var hair = message.Look[1];

        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)result);

            if (await _characters.CheckExistsByName(message.Name))
                result = LoginResult.InvalidCharacterName;

            if (result == LoginResult.Success)
            {
                ICharacter character = new Character
                {
                    AccountWorldID = message.User.AccountWorld!.ID,
                    Name = message.Name,
                    Job = 0, // TODO: race -> job
                    Face = face,
                    Hair = hair,
                    Skin = message.Skin,
                    Gender = message.Gender,
                    FieldID = 310000000, // TODO: start maps
                    FieldPortal = 0,
                    SubJob = 0 // TODO: race -> subjob
                };

                /*
                var context = new ModifyInventoryGroupContext(character.Inventories, _templateManager);
                context.SetEquipped(BodyPart.Clothes, message.Coat);
                context.SetEquipped(BodyPart.Shoes, message.Shoes);
                context.SetEquipped(BodyPart.Weapon, message.Weapon);
                if (message.Pants > 0)
                    context.SetEquipped(BodyPart.Pants, message.Pants);
                */

                character = await _characters.Insert(character);

                _logger.LogDebug(
                    "Created new {Race} character: {Name} (ID: {ID})",
                    message.Race, message.Name, character.ID
                );

                packet.WriteCharacterStats(character);
                packet.WriteCharacterLooks(character);

                packet.WriteBool(false);
                packet.WriteBool(false);
            }
            else
            {
                packet.WriteInt(0);
            }

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteInt(0);

            await message.User.Dispatch(packet);
        }
    }
}
