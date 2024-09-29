using System.Threading;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game.Contracts.Packets.Send;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;
using Edelstein.Protocol.Network.Packets;
using Edelstein.Protocol.Network.Packets.Types;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Reactors;

public class FieldReactor(
    IReactorTemplate template,
    IPoint2D position, 
    bool facingLeft = true,
    string? name = null
) : AbstractFieldObject(position), IFieldReactor
{
    public override FieldObjectType Type => FieldObjectType.Reactor;
    public IReactorTemplate Template => template;
    public string? Name => name;

    public byte State { get; private set; }
    
    private readonly SemaphoreSlim _lock = new(1, 1);
    
    public async Task SetState(byte state, short delay = 0, byte properEventIdx = 0, byte stateEnd = 0)
    {
        await _lock.WaitAsync();

        try
        {
            State = state;
            
            if (FieldSplit != null)
                await FieldSplit.Dispatch(new ReactorChangeState
                {
                    ObjectID = ObjectID ?? 0,
                    State = state,
                    Delay = delay,
                    ProperEventIDx = properEventIdx,
                    StateEnd = stateEnd
                });
        }
        finally
        {
            _lock.Release();
        }
    }

    public override IDispatchable GetDispatchEnterField(bool isEnterField = false) 
        => new ReactorEnterField
        {
            ObjectID = ObjectID ?? 0,
            TemplateID = template.ID,
            State = State,
            X = (short)position.X,
            Y = (short)position.Y,
            Flip = facingLeft,
            Name = new LPString(Name ?? "")
        };

    public override IDispatchable GetDispatchLeaveField(bool isLeaveField = false) 
        => new ReactorLeaveField
        {
            ObjectID = ObjectID ?? 0,
            State = State,
            X = (short)position.X,
            Y = (short)position.Y
        };
}
