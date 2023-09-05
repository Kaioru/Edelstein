using Edelstein.Protocol.Gameplay.Game.Objects.Drop;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Utilities.Spatial;

namespace Edelstein.Common.Gameplay.Game.Objects.Drop;

public class FieldDropMoney : AbstractFieldDrop
{
    public FieldDropMoney(
        IPoint2D position,
        int money,
        DropOwnType ownType = DropOwnType.NoOwn, 
        int ownerID = 0, 
        int sourceID = 0
    ) : base(position, ownType, ownerID, sourceID) 
        => Info = money;

    public override bool IsMoney => true;
    public override int Info { get; }

    protected override Task<bool> Check(IFieldUser user)
        => Task.FromResult(user.Character.Money + Info > 0);

    protected override Task Update(IFieldUser user)
        => user.ModifyStats(s => s.Money += Info);
}
