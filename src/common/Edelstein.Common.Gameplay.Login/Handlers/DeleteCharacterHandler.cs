using Edelstein.Common.Gameplay.Packets;
using Edelstein.Protocol.Gameplay.Login;
using Edelstein.Protocol.Gameplay.Login.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Login.Handlers;

public class DeleteCharacterHandler : IPacketHandler<ILoginStageUser>
{
    private readonly IPipeline<UserOnPacketDeleteCharacter> _pipeline;
   
    public short Operation => (short)PacketRecvOperations.DeleteCharacter;
    
    public DeleteCharacterHandler(IPipeline<UserOnPacketDeleteCharacter> pipeline) => _pipeline = pipeline;

    public bool Check(ILoginStageUser user) =>
        user.State == LoginState.SelectCharacter &&
        user.Account?.SPW != null;

    public Task Handle(ILoginStageUser user, IPacketReader reader)
    {
        var message = new UserOnPacketDeleteCharacter(
            user,
            reader.ReadString(),
            reader.ReadInt()
        );

        return _pipeline.Process(message);
    }
}
