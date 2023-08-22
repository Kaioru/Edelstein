using System.Collections.Concurrent;
using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Spatial;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Templates;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game;

public class FieldManager : IFieldManager
{
    private readonly IDictionary<int, IField> _fields;
    private readonly ITemplateManager<IFieldTemplate> _fieldTemplates;

    public FieldManager(
        ITickerManager tickerManager,
        ITemplateManager<IFieldTemplate> fieldTemplates
    )
    {
        _fields = new ConcurrentDictionary<int, IField>();
        _fieldTemplates = fieldTemplates;
    }

    public async Task<IField?> Retrieve(int key)
    {
        var field = _fields.TryGetValue(key, out var result) ? result : null;
        var template = await _fieldTemplates.Retrieve(key);

        if (field != null || template == null) return field;

        field = new Field(this, template);

        _fields.Add(key, field);

        return field;
    }

    public Task<ICollection<IField>> RetrieveAll() =>
        Task.FromResult<ICollection<IField>>(_fields.Values.ToImmutableList());
}
