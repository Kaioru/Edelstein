using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CreateNewCharacterPlug : IPipelinePlug<ICreateNewCharacter>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ILogger _logger;
    private readonly ITemplateManager<IItemTemplate> _templateManager;

    public CreateNewCharacterPlug(
        ILogger<CreateNewCharacterPlug> logger,
        ICharacterRepository characterRepository,
        ITemplateManager<IItemTemplate> templateManager
    )
    {
        _logger = logger;
        _characterRepository = characterRepository;
        _templateManager = templateManager;
    }

    public async Task Handle(IPipelineContext ctx, ICreateNewCharacter message)
    {
        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)result);

            if (await _characterRepository.CheckExistsByName(message.Name))
                result = LoginResult.InvalidCharacterName;

            if (result == LoginResult.Success)
            {
                ICharacter character = new Character
                {
                    AccountWorldID = message.User.AccountWorld!.ID,
                    Name = message.Name,
                    Job = 0, // TODO: race -> job
                    Face = message.Face,
                    Hair = message.Hair + message.HairColor,
                    Skin = (byte)message.Skin,
                    Gender = message.Gender,
                    FieldID = 310000000, // TODO: start maps
                    FieldPortal = 0,
                    SubJob = 0 // TODO: race -> subjob
                };
                var context = new ModifyInventoryGroupContext(character.Inventories, _templateManager);

                context.SetEquipped(BodyPart.Clothes, message.Coat);
                context.SetEquipped(BodyPart.Shoes, message.Shoes);
                context.SetEquipped(BodyPart.Weapon, message.Weapon);
                if (message.Pants > 0)
                    context.SetEquipped(BodyPart.Pants, message.Pants);

                character = await _characterRepository.Insert(character);

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
