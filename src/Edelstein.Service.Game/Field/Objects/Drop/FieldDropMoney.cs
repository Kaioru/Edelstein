using System.Threading.Tasks;
using Edelstein.Service.Game.Field.User;

namespace Edelstein.Service.Game.Field.Objects.Drop
{
    public class FieldDropMoney : AbstractFieldDrop
    {
        public override bool IsMoney => true;
        public override int Info => _money;

        private readonly int _money;

        public FieldDropMoney(int money)
            => _money = money;

        public override async Task PickUp(FieldUser user)
        {
            await Field.Leave(this, () => GetLeaveFieldPacket(0x2, user));
            await user.ModifyStats(s => s.Money += _money, true);
        }
    }
}