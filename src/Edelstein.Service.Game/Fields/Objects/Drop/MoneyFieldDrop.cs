using System.Threading.Tasks;
using Edelstein.Service.Game.Fields.Objects.User;
using Edelstein.Service.Game.Fields.Objects.User.Messages.Types.Drops;

namespace Edelstein.Service.Game.Fields.Objects.Drop
{
    public class MoneyFieldDrop : AbstractFieldDrop
    {
        public override bool IsMoney => true;
        public override int Info => _money;

        private readonly int _money;

        public MoneyFieldDrop(int money)
            => _money = money;

        public override async Task PickUp(FieldUser user)
        {
            await Field.Leave(this, () => GetLeaveFieldPacket(0x2, user));
            await user.ModifyStats(s => s.Money += _money, true);
            await user.Message(new MoneyDropPickUpMessage
            {
                Money = _money
            });
        }
    }
}