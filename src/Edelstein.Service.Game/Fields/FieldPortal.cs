using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields;
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

        public Task Leave(IFieldUser user)
            => _field.Enter(user, (byte) _template.ID);
    }
}