using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

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
        => new UserOnPacketDeleteCharacter(
            user,
            reader.ReadString(),
            reader.ReadInt()
        );
}
