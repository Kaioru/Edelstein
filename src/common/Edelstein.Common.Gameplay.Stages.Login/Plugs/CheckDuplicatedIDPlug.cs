using Edelstein.Common.Gameplay.Characters;
using Edelstein.Common.Gameplay.Packets;
using Edelstein.Common.Util.Buffers.Packets;
using Edelstein.Protocol.Gameplay.Stages.Login.Contracts;
using Edelstein.Protocol.Util.Pipelines;

namespace Edelstein.Common.Gameplay.Stages.Login.Plugs;

public class CheckDuplicatedIDPlug : IPipelinePlug<ICheckDuplicatedID>
{
    private readonly ICharacterRepository _characterRepository;

    public CheckDuplicatedIDPlug(ICharacterRepository characterRepository) =>
        _characterRepository = characterRepository;

    public async Task Handle(IPipelineContext ctx, ICheckDuplicatedID message)
    {
        var result = await _characterRepository.CheckExistsByName(message.Name);
        var packet = new PacketWriter(PacketSendOperations.CheckDuplicatedIDResult);

        packet.WriteString(message.Name);
        packet.WriteBool(result);

        await message.User.Dispatch(packet);
    }
}
