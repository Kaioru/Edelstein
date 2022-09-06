using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Characters;
using Edelstein.Protocol.Gameplay.Inventories.Templates;
using Edelstein.Protocol.Gameplay.Stages.Login;
using Edelstein.Protocol.Util.Buffers.Packets;
using Edelstein.Protocol.Util.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Handlers;

public class CreateNewCharacterHandler : AbstractLoginPacketHandler
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ILogger _logger;
    private readonly ITemplateManager<IItemTemplate> _templateManager;


    public CreateNewCharacterHandler(
        ILogger<CreateNewCharacterHandler> logger,
        ICharacterRepository characterRepository,
        ITemplateManager<IItemTemplate> templateManager
    )
    {
        _logger = logger;
        _characterRepository = characterRepository;
        _templateManager = templateManager;
    }

    public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override async Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var name = reader.ReadString();
        _ = reader.ReadInt();
        _ = reader.ReadInt();
        var race = reader.ReadInt();
        var subJob = reader.ReadShort();
        var gender = reader.ReadByte();
        var skin = reader.ReadByte();

        var items = new int[reader.ReadByte()];

        for (var i = 0; i < items.Length; i++)
            items[i] = reader.ReadInt();

        var face = items[0];
        var hair = items[1];

        try
        {
            var result = LoginResult.Success;
            var packet = new PacketWriter();

            packet.WriteShort(11);

            packet.WriteByte((byte)result);

            if (await _characterRepository.CheckExistsByName(name))
                result = LoginResult.InvalidCharacterName;

            if (result == LoginResult.Success)
            {
                ICharacter character = new Character
                {
                    AccountWorldID = user.AccountWorld!.ID,
                    Name = name,
                    Job = 0, // TODO: race -> job
                    Face = face,
                    Hair = hair,
                    Skin = skin,
                    Gender = gender,
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

                character = await _characterRepository.Insert(character);

                _logger.LogDebug(
                    "Created new {Race} character: {Name} (ID: {ID})",
                    race, name, character.ID
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

            await user.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new PacketWriter();

            packet.WriteShort(11);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteInt(0);

            await user.Dispatch(packet);
        }
    }
}
