using System.Linq;
using Edelstein.Service.Game.Conversations.Speakers.Fields.Continents;
using Edelstein.Service.Game.Fields;
using Edelstein.Service.Game.Fields.Objects;
using Edelstein.Service.Game.Fields.User;

namespace Edelstein.Service.Game.Conversations.Speakers.Fields
{
    public class FieldSpeaker : Speaker
    {
        private readonly IField _field;

        public FieldSpeaker(IConversationContext context, IField field) : base(context)
            => _field = field;

        public ContinentSpeaker GetContinent()
            => new ContinentSpeaker(Context, Context.Socket.Service.ContinentManager.Continents.First(c =>
                c.Template.StartShipMoveFieldID == _field.ID ||
                c.Template.WaitFieldID == _field.ID ||
                c.Template.MoveFieldID == _field.ID));

        public FieldUserSpeaker GetUser(int id)
            => new FieldUserSpeaker(Context, _field.GetObject<FieldUser>(id));

        public FieldNPCSpeaker GetNpc(int template)
            => new FieldNPCSpeaker(Context, _field
                .GetObjects<FieldNPC>()
                .First(npc => npc.Template.ID == template));

        public FieldPortalSpeaker GetPortal(string portal)
            => new FieldPortalSpeaker(Context, _field.GetPortal(portal));
    }
}