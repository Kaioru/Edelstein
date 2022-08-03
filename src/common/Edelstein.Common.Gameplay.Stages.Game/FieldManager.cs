using System.Collections.Concurrent;
using Edelstein.Protocol.Gameplay.Stages.Game;
using Edelstein.Protocol.Gameplay.Stages.Game.Templates;
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
}
