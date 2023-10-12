using Edelstein.Common.Gameplay.Handling.Packets;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Pipelines;

namespace Edelstein.Common.Gameplay.Game.Handling.Packets;

public abstract class AbstractPipedFieldHandler<TMessage> : AbstractPipedPacketHandler<IGameStageUser, TMessage>
{
    protected AbstractPipedFieldHandler(IPipeline<TMessage> pipeline) : base(pipeline)
    {
    }
    
    public override bool Check(IGameStageUser user) 
        => user is { Field: not null, FieldUser: not null };
    
    public override TMessage? Serialize(IGameStageUser user, IPacketReader reader) 
        => Serialize(user.FieldUser!, reader);
    
    protected abstract TMessage? Serialize(IFieldUser user, IPacketReader reader);
}
