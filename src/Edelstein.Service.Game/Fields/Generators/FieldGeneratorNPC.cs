using System.Threading.Tasks;
using Edelstein.Core.Templates.Fields.Life;
using Edelstein.Core.Templates.NPC;
using Edelstein.Service.Game.Fields.Objects.NPC;

namespace Edelstein.Service.Game.Fields.Generators
{
    public class FieldGeneratorNPC : AbstractFieldGenerator
    {
        private readonly FieldLifeTemplate _lifeTemplate;
        private readonly NPCTemplate _npcTemplate;

        public FieldGeneratorNPC(FieldLifeTemplate lifeTemplate, NPCTemplate npcTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _npcTemplate = npcTemplate;
        }

        public override bool Available(IField field, bool reset = false)
            => Generated.Count == 0;

        public override async Task Generate(IField field)
        {
            var npc = new FieldNPC(_npcTemplate, _lifeTemplate.Left)
            {
                Generator = this,
                Position = _lifeTemplate.Position,
                Foothold = (short) _lifeTemplate.FH,
                RX0 = _lifeTemplate.RX0,
                RX1 = _lifeTemplate.RX1
            };

            Generated.Add(npc);
            await field.Enter(npc);
        }

        public override async Task Reset(IFieldGeneratorObj obj)
        {
            obj.Generator = null;
            Generated.Remove(obj);
        }
    }
}