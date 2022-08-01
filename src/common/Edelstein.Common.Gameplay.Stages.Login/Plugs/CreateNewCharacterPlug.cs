using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Gameplay.Stages.Login.Types;
using Edelstein.Common.Util.Buffers.Bytes;
using Edelstein.Protocol.Gameplay.Stages.Login.Messages;
using Edelstein.Protocol.Util.Pipelines;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CreateNewCharacterPlug : IPipelinePlug<ICreateNewCharacter>
{
    private readonly ICharacterRepository _characterRepository;
    private readonly ILogger _logger;

    public CreateNewCharacterPlug(ILogger<CreateNewCharacterPlug> logger, ICharacterRepository characterRepository)
    {
        _logger = logger;
        _characterRepository = characterRepository;
    }

    public async Task Handle(IPipelineContext ctx, ICreateNewCharacter message)
    {
        try
        {
            var result = LoginResult.Success;
            var packet = new ByteWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)result);

            var character = new Character
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

            message.User.Character = await _characterRepository.Insert(character);

            _logger.LogDebug(
                "Created new {Race} character: {Name} (ID: {ID})",
                message.Race, message.Name, message.User.Character.ID
            );

            // TODO: WriteCharacterStats
            // TODO: WriteCharacterLook

            packet.WriteBool(false);
            packet.WriteBool(false);

            await message.User.Dispatch(packet);
        }
        catch (Exception)
        {
            var packet = new ByteWriter(PacketSendOperations.CreateNewCharacterResult);

            packet.WriteByte((byte)LoginResult.DBFail);
            packet.WriteInt(0);

            await message.User.Dispatch(packet);
        }
    }
}
