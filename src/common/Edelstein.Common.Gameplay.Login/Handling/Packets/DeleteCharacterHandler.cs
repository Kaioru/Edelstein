using Edelstein.Common.Gameplay.Handling;
using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handling.Packets;

public class DeleteCharacterHandler : AbstractPipedPacketHandler<ILoginStageUser, UserOnPacketDeleteCharacter>
{
    public DeleteCharacterHandler(IPipeline<UserOnPacketDeleteCharacter> pipeline) : base(pipeline)
    {
    }
    
    public override short Operation => (short)PacketRecvOperations.DeleteCharacter;

    public override bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public override UserOnPacketDeleteCharacter Serialize(ILoginStageUser user, IPacketReader reader)
        => new(
            user,
            reader.ReadString(),
            reader.ReadInt()
        );
}
