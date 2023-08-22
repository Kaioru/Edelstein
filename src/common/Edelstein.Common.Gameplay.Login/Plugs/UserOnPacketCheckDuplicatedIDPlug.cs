using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Utilities.Packets;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Models.Characters;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Plugs;

public class UserOnPacketCheckDuplicatedIDPlug : IPipelinePlug<UserOnPacketCheckDuplicatedID>
{
    private readonly ICharacterRepository _characterRepository;

    public UserOnPacketCheckDuplicatedIDPlug(ICharacterRepository characterRepository) =>
        _characterRepository = characterRepository;

    public async Task Handle(IPipelineContext ctx, UserOnPacketCheckDuplicatedID message)
    {
        var result = await _characterRepository.CheckExistsByName(message.Name);
        var packet = new PacketWriter(PacketSendOperations.CheckDuplicatedIDResult);

        packet.WriteString(message.Name);
        packet.WriteBool(result);

        await message.User.Dispatch(packet.Build());
    }
}
