using System.Linq;
using Edelstein.Service.Game.Fields;

namespace Edelstein.Service.Game.Conversations.Fields
{
    public class FieldSpeaker : AbstractSpeaker
    {
        public override byte TypeID => 0;
        public override int TemplateID => 9010000;
        public override ScriptMessageParam Param => 0;

        private readonly IField _field;

        public FieldSpeaker(IConversationContext context, IField field) : base(context)
            => _field = field;

        public ISpeaker AsPortal(int id)
            => _field.Template.Portals.ContainsKey(id)
                ? new FieldPortalSpeaker(Context, _field.Template.Portals[id], _field)
                : null;

        public ISpeaker AsPortal(string name)
        {
            var portal = _field.Template.Portals.Values
                .FirstOrDefault(p => p.Name == name);
            return portal == null ? null : AsPortal(portal.ID);
        }
    }
}