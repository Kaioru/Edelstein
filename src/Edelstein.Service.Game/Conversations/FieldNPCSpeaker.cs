using Edelstein.Service.Game.Field.Objects;

namespace Edelstein.Service.Game.Conversations
{
    public class FieldNPCSpeaker : AbstractFieldObjSpeaker<FieldNPC>
    {
        public override byte TypeID => 0;
        public override int TemplateID => Obj.Template.ID;
        public override ScriptMessageParam Param => 0;
        
        public FieldNPCSpeaker(IConversationContext context, FieldNPC npc) : base(context, npc)
        {
        }

        public string InitRegistry(string key, string value)
        {
            if (!ContainsRegistry(key))
                Obj.Registry[key] = value;
            return GetRegistry(key);
        }

        public string SetRegistry(string key, string value)
        {
            Obj.Registry[key] = value;
            return value;
        }

        public bool ContainsRegistry(string key)
            => Obj.Registry.ContainsKey(key);

        public string GetRegistry(string key)
            => !ContainsRegistry(key) ? string.Empty : Obj.Registry[key];
    }
}