using Edelstein.Common.Gameplay.Login.Types;
using Edelstein.Common.Gameplay.Models.Characters;
using Edelstein.Common.Gameplay.Models.Inventories.Modify;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Login.Types;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Gameplay.Models.Inventories.Modify;
using Edelstein.Protocol.Gameplay.Models.Inventories.Templates;
using Edelstein.Protocol.Utilities.Pipelines;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketCreateNewCharacterPlug : IPipelinePlug<UserOnPacketCreateNewCharacter>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ILogger _logger;
    private readonly ITemplateManager<IItemTemplate> _templateManager;

    public UserOnPacketCreateNewCharacterPlug(
        ILogger<UserOnPacketCreateNewCharacter> logger,
        ICharacterRepository characterRepository,
        ITemplateManager<IItemTemplate> templateManager
    )
    {
        _logger = logger;
        _characterRepository = characterRepository;
        _templateManager = templateManager;
    }

    public async Task Handle(IPipelineContext ctx, UserOnPacketCreateNewCharacter message)
    {
        try
        {
            var result = LoginResult.Success;
            using var packet = new PacketWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)result);

            if (await _characterRepository.CheckExistsByName(message.Name))
                result = LoginResult.InvalidCharacterName;

            if (result == LoginResult.Success)
            {
                ICharacter character = new Character
                {
                    AccountWorldID = message.User.AccountWorld!.ID,
                    Name = message.Name,
                    Job = message.Race switch
                    {
                        RaceSelectType.Resistance => 3000,
                        RaceSelectType.Normal => 0,
                        RaceSelectType.Cygnus => 1000,
                        RaceSelectType.Aran => 2000,
                        RaceSelectType.Evan => 2001,
                        _ => 0
                    },
                    Face = message.Face,
                    Hair = message.Hair + message.HairColor,
                    Skin = (byte)message.Skin,
                    Gender = message.Gender,
                    FieldID = message.Race switch
                    {
                        RaceSelectType.Resistance => 931000000,
                        RaceSelectType.Normal => 0,
                        RaceSelectType.Cygnus => 130030000,
                        RaceSelectType.Aran => 914000000,
                        RaceSelectType.Evan => 900010000,
                        _ => 0
                    },
                    FieldPortal = 0,
                    SubJob = (short)(message.Race == RaceSelectType.Normal ? message.SubJob > 0 ? 1 : 0 : 0)
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

            await message.User.Dispatch(packet.Build());
        }
        catch (Exception)
        {
            using var packet = new PacketWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteInt(0);

            await message.User.Dispatch(packet.Build());
        }
    }
}
