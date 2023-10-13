using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Gameplay.Login.Types;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class CreateNewCharacterHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketCreateNewCharacter>
{
    public CreateNewCharacterHandler(IPipeline<UserOnPacketCreateNewCharacter> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.CreateNewCharacter;

    public override bool Check(ILoginStageUser user) => user.State == LoginState.SelectCharacter;

    public override UserOnPacketCreateNewCharacter Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadString(),
            (RaceSelectType)reader.ReadInt(),
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
