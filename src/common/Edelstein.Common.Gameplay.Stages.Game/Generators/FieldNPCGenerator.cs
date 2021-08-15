using System;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Generators;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game.Generators
{
    public class FieldNPCGenerator : IFieldGenerator
    {
        private readonly FieldLifeTemplate _lifeTemplate;
        private readonly NPCTemplate _npcTemplate;

        private IFieldObjNPC NPC { get; set; }

        public FieldNPCGenerator(FieldLifeTemplate lifeTemplate, NPCTemplate npcTemplate)
        {
            _lifeTemplate = lifeTemplate;
            _npcTemplate = npcTemplate;
        }

        public bool Check(DateTime now, IField field)
            => NPC == null;

        public async Task Generate(IField field)
        {
            NPC = new FieldObjNPC(_npcTemplate, _lifeTemplate.Left)
            {
                Position = _lifeTemplate.Position,
                Foothold = field.GetFoothold(_lifeTemplate.FH),
                RX0 = _lifeTemplate.RX0,
                RX1 = _lifeTemplate.RX1
            };

            await field.Enter(NPC);
        }
    }
}
