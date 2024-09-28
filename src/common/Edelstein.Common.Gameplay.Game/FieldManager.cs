using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Common.Gameplay.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Game.Objects.Reactors;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Reactors.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game;

public class FieldManager(
    ITemplateManager<IFieldTemplate> templates,
    ITemplateManager<INPCTemplate> npcs,
    ITemplateManager<IMobTemplate> mobs,
    ITemplateManager<IReactorTemplate> reactors
) : IFieldManager
{
    private readonly ConcurrentDictionary<int, IField> _fields = new();
    
    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await templates.Retrieve(key);

        if (field != null || template == null) return field;

        field = new Field(template);

        foreach (var life in template.Life)
            switch (life.Type)
            {
                case FieldLifeType.NPC:
                {
                    var npcTemplate = await npcs.Retrieve(life.TemplateID);
                    if (npcTemplate == null) continue;
                    var npc = new FieldNPC(
                        npcTemplate,
                        life.Position,
                        await template.Footholds.Retrieve(life.Foothold),
                        life.Bounds,
                        life.IsFacingLeft
                    );

                    await field.Enter(npc);
                    break;
                }
                case FieldLifeType.Monster:
                {
                    var mobTemplate = await mobs.Retrieve(life.TemplateID);
                    if (mobTemplate == null) continue;
                    var mob = new FieldMob(
                        mobTemplate,
                        life.Position,
                        await template.Footholds.Retrieve(life.Foothold),
                        await template.Footholds.Retrieve(life.Foothold),
                        life.IsFacingLeft
                    );

                    await field.Enter(mob);
                    break;
                }
            }
        
        foreach (var reactor in template.Reactors)
        {
            var reactorTemplate = await reactors.Retrieve(reactor.TemplateID);
            if (reactorTemplate == null) continue;
            var reactorObj = new FieldReactor(
                reactorTemplate,
                reactor.Position
            );

            await field.Enter(reactorObj);
        }
        
        return _fields.TryAdd(key, field) ? field : null;
    }
    
    public Task<ICollection<IField>> RetrieveAll() 
        => Task.FromResult(_fields.Values);
}
