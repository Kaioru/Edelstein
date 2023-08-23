using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class CreateNewCharacterHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCreateNewCharacter>
{
    public CreateNewCharacterHandler(IPipeline<UserOnPacketCreateNewCharacter?> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override UserOnPacketCreateNewCharacter Serialize(ILoginStageUser user, IPacketReader reader)
        => new UserOnPacketCreateNewCharacter(
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
}
