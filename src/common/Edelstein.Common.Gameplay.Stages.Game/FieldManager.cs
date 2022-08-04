using System.Collections.Concurrent;
using Edelstein.Common.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Common.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Gameplay.Stages.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Stages.Game.Spatial;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
using Edelstein.Protocol.Util.Spatial;
using Edelstein.Protocol.Util.Templates;

namespace Edelstein.Common.Gameplay.Stages.Game;

public class FieldManager : IFieldManager
{
    private readonly IDictionary<int, IField> _fields;
    private readonly ITemplateManager<IFieldTemplate> _templates;

    public FieldManager(ITemplateManager<IFieldTemplate> templates)
    {
        _fields = new ConcurrentDictionary<int, IField>();
        _templates = templates;
    }

    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await _templates.Retrieve(key);

        if (field != null || template == null) return field;

        field = new Field(template);
        _fields.Add(key, field);

        return field;
    }
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
        int rx0 = 0,
        int rx1 = 0,
        bool isFacingLeft = true,
        bool isEnabled = true
    ) => new FieldNPC(template, position, foothold, rx0, rx1, isFacingLeft, isEnabled);
}
