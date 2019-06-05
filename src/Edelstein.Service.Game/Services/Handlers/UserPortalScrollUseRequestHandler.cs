using System.Threading.Tasks;
using Edelstein.Core;
using Edelstein.Database.Entities.Inventories.Items;
using Edelstein.Network.Packets;
using Edelstein.Provider.Templates.Item.Consume;
using Edelstein.Service.Game.Fields.Objects.User;

namespace Edelstein.Service.Game.Services.Handlers
{
    public class UserPortalScrollUseRequestHandler : AbstractItemUseHandler<PortalScrollItemTemplate>
    {
        public override async Task Handle(
            RecvPacketOperations operation,
            IPacket packet,
            FieldUser user,
            PortalScrollItemTemplate template,
            ItemSlot item
        )
        {
            var fieldID = template.MoveTo == 999999999
                ? user.Field.Template.FieldReturn ?? user.Field.ID
                : template.MoveTo;
            
            var field = user.Service.FieldManager.Get(fieldID);

            await field.Enter(user, 0);
        }
    }
}