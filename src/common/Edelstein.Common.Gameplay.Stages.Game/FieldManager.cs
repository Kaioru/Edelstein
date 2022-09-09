using System.Collections.Concurrent;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Stages.Game.Generators;
using Edelstein.Common.Gameplay.Stages.Game.Objects.MessageBox;
using Edelstein.Common.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Contexts;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.MessageBox;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Templates;
using Edelstein.Protocol.Util.Tickers;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class FieldManager : IFieldManager, ITickable
{
    private readonly IGameContextEvents _events;
    private readonly IDictionary<int, IField> _fields;
    private readonly ITemplateManager<IFieldTemplate> _fieldTemplates;
    private readonly ITemplateManager<IMobTemplate> _mobTemplates;
    private readonly ITemplateManager<INPCTemplate> _npcTemplates;

    public FieldManager(
        ITickerManager tickerManager,
        ITemplateManager<IFieldTemplate> fieldTemplates,
        ITemplateManager<INPCTemplate> npcTemplates,
        ITemplateManager<IMobTemplate> mobTemplates,
        IGameContextEvents events
    )
    {
        _fields = new ConcurrentDictionary<int, IField>();
        _fieldTemplates = fieldTemplates;
        _npcTemplates = npcTemplates;
        _mobTemplates = mobTemplates;
        _events = events;

        tickerManager.Schedule(new FieldGeneratorTicker(this), TimeSpan.FromSeconds(7));
    }

    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await _fieldTemplates.Retrieve(key);

        if (field != null || template == null) return field;

        field = CreateField(template);

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

        field.Generators.Add(new FieldGeneratorNPC(npcUnits));
        //field.Generators.Add(new FieldGeneratorMob(field, mobUnits));

        foreach (var generator in field.Generators)
        foreach (var obj in generator.Generate())
            await field.Enter(obj);

        _fields.Add(key, field);

        return field;
    }

    public Task<IEnumerable<IField>> RetrieveAll() =>
        Task.FromResult<IEnumerable<IField>>(_fields.Values.ToImmutableList());

    public IField CreateField(IFieldTemplate template) =>
        new Field(this, template, _events);

    public IFieldUser? CreateUser(IGameStageUser user)
    {
        if (user.Account == null || user.AccountWorld == null || user.Character == null)
            return null;
        return new FieldUser(user, user.Account!, user.AccountWorld!, user.Character!);
    }

    public IFieldNPC CreateNPC(
        INPCTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        IRectangle2D? bounds = null,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) => new FieldNPC(template, position, foothold, bounds, isFacingLeft, isEnabled);


    public IFieldMob CreateMob(
        IMobTemplate template,
        IPoint2D position,
        IFieldFoothold? foothold = null,
        bool isFacingLeft = true
    ) => new FieldMob(template, position, foothold, isFacingLeft);

    public IFieldMessageBox CreateMessageBox(
        IPoint2D position,
        int itemID,
        string hope,
        string name
    ) => new FieldMessageBox(position, itemID, hope, name);

    public Task OnTick(DateTime now) => Task.CompletedTask;
}
