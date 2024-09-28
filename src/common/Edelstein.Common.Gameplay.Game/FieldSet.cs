using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.Users;
using Edelstein.Protocol.Network.Packets;

namespace Edelstein.Common.Gameplay.Game;

public class FieldSet(
    string id
) : IFieldSet
{
    public string ID { get; } = id;

    private HashSet<IField> _fields { get; } = new();
    
    protected IField? FieldEnter { get; set; }
    protected IField? FieldLeave { get; set; }

    public IFieldObject? GetObject(int id)
        => _fields
            .SelectMany(f => f.GetObjects())
            .FirstOrDefault(o => o.ObjectID == id);

    public IEnumerable<IFieldObject> GetObjects()
        => _fields
            .SelectMany(f => f.GetObjects())
            .ToImmutableList();

    public Task Dispatch(IDispatchable dispatch, IFieldObject? source = null)
        => Task.WhenAll(GetObjects()
            .OfType<IFieldUser>()
            .Where(u => u != source)
            .Select(u => u.Dispatch(dispatch)));

    public virtual Task Initialize(IFieldManager manager) => Task.CompletedTask;

    public async Task Enter(IFieldObject obj)
    {
        if (FieldEnter != null)
            await FieldEnter.Enter(obj);
    }
    
    public async Task Leave(IFieldObject obj)
    {
        if (FieldLeave != null)
            await FieldLeave.Enter(obj);
    }
    
    public virtual Task OnUserEnter(IFieldUser user) => Task.CompletedTask;
    public virtual Task OnUserLeave(IFieldUser user) => Task.CompletedTask;
    public virtual Task OnUserMigrate(IFieldUser user, IField from, IField to) => Task.CompletedTask;

    public virtual Task Reset() => Task.CompletedTask;
    
    public void Dispose()
    {
        foreach (var field in _fields)
            field.FieldSet = null;
    }
    
    protected IField? Register(IField? field)
    {
        if (field == null) return null;

        field.FieldSet = this;
        _fields.Add(field);
        return field;
    }
}
