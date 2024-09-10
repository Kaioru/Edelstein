using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game;

public class FieldManager(
    ITemplateManager<IFieldTemplate> templates
) : IFieldManager
{
    private readonly ConcurrentDictionary<int, IField> _fields = new();
    
    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await templates.Retrieve(key);

        if (field != null || template == null) return field;

        field = new Field(template);

        return _fields.TryAdd(key, field) ? field : null;
    }
    
    public Task<ICollection<IField>> RetrieveAll() 
        => Task.FromResult(_fields.Values);
}
