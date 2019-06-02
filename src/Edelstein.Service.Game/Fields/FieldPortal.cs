using System.Threading.Tasks;
using Edelstein.Provider.Templates.Field;
using Edelstein.Service.Game.Fields.Objects;

namespace Edelstein.Service.Game.Fields
{
    public class FieldPortal : IFieldPortal
    {
        private readonly IField _field;
        private readonly FieldPortalTemplate _template;

        public FieldPortal(IField field, FieldPortalTemplate template)
        {
            _field = field;
            _template = template;
        }

        public async Task Enter(IFieldUser user)
        {
            if (_template.ToMap != 999999999)
            {
                var to = user.Service.FieldManager.Get(_template.ToMap);
                var portal = to.GetPortal(_template.ToName);

                await portal.Leave(user);
            }
        }

        public async Task Leave(IFieldUser user)
        {
            await _field.Enter(user, (byte) _template.ID);
        }
    }
}