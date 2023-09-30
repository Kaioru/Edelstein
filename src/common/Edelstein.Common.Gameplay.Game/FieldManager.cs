using System.Collections.Concurrent;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Generators;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game;

public class FieldManager : IFieldManager
{
    private readonly IDictionary<int, IField> _fields;
    private readonly ITemplateManager<IFieldTemplate> _fieldTemplates;
    private readonly ITemplateManager<IMobTemplate> _mobTemplates;
    private readonly ITemplateManager<INPCTemplate> _npcTemplates;

    public FieldManager(
        ITickerManager tickerManager,
        ITemplateManager<IFieldTemplate> fieldTemplates,
        ITemplateManager<IMobTemplate> mobTemplates,
        ITemplateManager<INPCTemplate> npcTemplates
    )
    {
        _fields = new ConcurrentDictionary<int, IField>();
        _fieldTemplates = fieldTemplates;
        _mobTemplates = mobTemplates;
        _npcTemplates = npcTemplates;
    }

    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await _fieldTemplates.Retrieve(key);

        if (field != null || template == null) return field;

        field = new Field(this, template);

        var npcUnits = new List<IFieldGeneratorUnit>();
        var mobUnits = new List<IFieldGeneratorUnit>();

        foreach (var life in template.Life)
            switch (life.Type)
            {
                case FieldLifeType.NPC:
                {
                    var npc = await _npcTemplates.Retrieve(life.ID);
                    if (npc == null) continue;
                    npcUnits.Add(new FieldGeneratorNPCUnit(field, life, npc));
                    break;
                }
                case FieldLifeType.Monster:
                {
                    var mob = await _mobTemplates.Retrieve(life.ID);
                    if (mob == null) continue;
                    mobUnits.Add(life.MobTime > 0
                        ? new FieldGeneratorMobTimedUnit(field, life, mob)
                        : new FieldGeneratorMobNormalUnit(field, life, mob)
                    );
                    break;
                }
            }

        await field.Generators.Insert(new FieldGeneratorNPC("default-npc", field, npcUnits));
        await field.Generators.Insert(new FieldGeneratorMob("default-mob", field, mobUnits));

        _fields.Add(key, field);

        return field;
    }

    public Task<ICollection<IField>> RetrieveAll() =>
        Task.FromResult<ICollection<IField>>(_fields.Values.ToImmutableHashSet());
}
