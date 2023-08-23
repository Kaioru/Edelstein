using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CreateNewCharacterHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketCreateNewCharacter> _pipeline;

    public CreateNewCharacterHandler(IPipeline<UserOnPacketCreateNewCharacter> pipeline) => _pipeline = pipeline;

    public short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketCreateNewCharacter(
            user,
            reader.ReadString(),
            reader.ReadInt(),
            reader.ReadShort(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadInt(),
            reader.ReadByte()
        );

        return _pipeline.Process(message);
    }
}
