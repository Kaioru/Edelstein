using System.Collections.Immutable;
using Edelstein.Common.Gameplay.Game.Conversations;
using Edelstein.Common.Gameplay.Game.Conversations.Speakers;
using Edelstein.Common.Gameplay.Game.Generators;
using Edelstein.Common.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game;
using Edelstein.Protocol.Gameplay.Game.Conversations;
using Edelstein.Protocol.Gameplay.Game.Conversations.Speakers;
using Edelstein.Protocol.Gameplay.Game.Generators;
using Edelstein.Protocol.Gameplay.Game.Objects;
using Edelstein.Protocol.Gameplay.Game.Objects.User;
using Edelstein.Protocol.Gameplay.Game.Templates;
using Edelstein.Protocol.Services.Social.Contracts;
using Edelstein.Protocol.Utilities.Packets;
using Edelstein.Protocol.Utilities.Spatial;
using Edelstein.Protocol.Utilities.Tickers;

namespace Edelstein.Common.Gameplay.Game;

public class Field : AbstractFieldObjectPool, IField, ITickable
{
    private const int ScreenWidth = 1024;
    private const int ScreenHeight = 768;
    private const int ScreenWidthOffset = ScreenWidth * 75 / 100;
    private const int ScreenHeightOffset = ScreenHeight * 75 / 100;

    private readonly IDictionary<FieldObjectType, FieldObjectPool> _pools;
    private readonly IFieldSplit[,] _splits;

    public Field(IFieldManager manager, IFieldTemplate template)
    {
        Manager = manager;
        Template = template;

        Generators = new FieldGeneratorRegistry();

        _pools = new Dictionary<FieldObjectType, FieldObjectPool>();
        foreach (var type in Enum.GetValues<FieldObjectType>())
            _pools[type] = new FieldObjectPool();

        var splitRowCount = (int)(template.Bounds.Height + (ScreenHeightOffset - 1)) / ScreenHeightOffset;
        var splitColCount = (int)(template.Bounds.Width + (ScreenWidthOffset - 1)) / ScreenWidthOffset;

        _splits = new IFieldSplit[splitRowCount, splitColCount];

        for (var row = 0; row < splitRowCount; row++)
        for (var col = 0; col < splitColCount; col++)
            _splits[row, col] = new FieldSplit(row, col);
    }

    public int ID => Template.ID;

    public IFieldManager Manager { get; }
    public IFieldTemplate Template { get; }

    public IFieldGeneratorRegistry Generators { get; }
    
    private DateTime NextGeneratorTick { get; set; }

    public override IReadOnlyCollection<IFieldObject> Objects =>
        _pools.Values.SelectMany(p => p.Objects).ToImmutableList();

    public IFieldSplit? GetSplit(IPoint2D position)
    {
        var row = (position.Y - Template.Bounds.Top) / ScreenHeightOffset;
        var col = (position.X - Template.Bounds.Left) / ScreenWidthOffset;
        return GetSplit(row, col);
    }
    
    public IFieldSplit?[] GetSplits(IRectangle2D bounds)
    {
        var minRow = (bounds.Top - Template.Bounds.Top) / ScreenHeightOffset;
        var maxRow = (bounds.Bottom - Template.Bounds.Top) / ScreenHeightOffset;
        var minCol = (bounds.Left - Template.Bounds.Left) / ScreenWidthOffset;
        var maxCol = (bounds.Right - Template.Bounds.Left) / ScreenWidthOffset;
        var splits = new IFieldSplit?[(maxRow - minRow + 1) * (maxCol - minCol + 1)];
        var index = 0;
        
        for (var row = minRow; row <= maxRow; row++)
        for (var col = minCol; col <= maxCol; col++)
            splits[index++] = GetSplit(row, col);
        return splits;
    }

    public IFieldSplit?[] GetEnclosingSplits(IPoint2D position)
    {
        var row = (position.Y - Template.Bounds.Top) / ScreenHeightOffset;
        var col = (position.X - Template.Bounds.Left) / ScreenWidthOffset;
        return GetEnclosingSplits(row, col);
    }

    public IFieldSplit?[] GetEnclosingSplits(IFieldSplit split) =>
        GetEnclosingSplits(split.Row, split.Col);

    public IFieldObjectPool? GetPool(FieldObjectType type) =>
        _pools.TryGetValue(type, out var pool)
            ? pool
            : null;

    public override Task Enter(IFieldObject obj) => Enter(obj, null);
    public override Task Leave(IFieldObject obj) => Leave(obj, null);

    public Task Enter(IFieldUser user) => Enter(user, 0);
    public Task Leave(IFieldUser user) => Leave(user, null);

    public async Task Enter(IFieldUser user, byte portal, Func<IPacket>? getEnterPacket = null)
    {
        user.Character.FieldPortal = portal;
        await Enter((IFieldObject)user, null);
    }

    public async Task Enter(IFieldUser user, string portal, Func<IPacket>? getEnterPacket = null)
    {
        user.Character.FieldPortal = (byte)(Template.Portals.Objects
            .FirstOrDefault(o => o.Name == portal)?
            .ID ?? 0);
        await Enter((IFieldObject)user, null);
    }

    public async Task Enter(IFieldObject obj, Func<IPacket>? getEnterPacket)
    {
        var pool = GetPool(obj.Type);

        if (obj.Field != null)
            await obj.Field.Leave(obj);
        obj.Field = this;

        if (obj is IFieldUser user)
        {
            var isFirstUser = !Objects.OfType<IFieldUser>().Any();
            var portal =
                Template.Portals.FindByID(user.Character.FieldPortal) ??
                Template.Portals.FindClosest(obj.Position).FirstOrDefault();

            user.Character.FieldID = ID;
            if (portal != null)
                await user.Move(
                    portal.Position, 
                    Template.Footholds.Find(portal.Position).FirstOrDefault(), 
                    true
                );

            await user.Dispatch(user.GetSetFieldPacket());
            
            if (Template.ScriptFirstUserEnter != null || Template.ScriptUserEnter != null)
            {
                var script = isFirstUser ? Template.ScriptFirstUserEnter ?? Template.ScriptUserEnter : Template.ScriptUserEnter;
                
                if (script != null)
                {
                    var conversation = await user.StageUser.Context.Managers.Conversation.Retrieve(script) as IConversation ??
                                       new FallbackConversation(script, user);

                    _ = user.Converse(
                        conversation,
                        c => new ConversationSpeaker(c),
                        c => new ConversationSpeakerUser(user, c, flags: ConversationSpeakerFlags.NPCReplacedByUser)
                    );
                }
            }

            if (user.IsInstantiated)
            {
                if (user.StageUser.Party != null)
                    _ = user.StageUser.Context.Services.Party.UpdateChannelOrField(new PartyUpdateChannelOrFieldRequest(
                        user.StageUser.Party.ID,
                        user.Character.ID,
                        user.StageUser.Context.Options.ChannelID,
                        ID
                    ));
            }

            if (!user.IsInstantiated) user.IsInstantiated = true;
        }

        var split = GetSplit(obj.Position);

        if (pool != null) await pool.Enter(obj);
        if (split != null) await split.Enter(obj, getEnterPacket);
        
        if (obj is IFieldUser owner)
            foreach (var owned in owner.Owned)
            {
                await owned.Move(owner.Position, owner.Foothold);
                await Enter(owned);
            }
    }

    public async Task Leave(IFieldObject obj, Func<IPacket>? getLeavePacket)
    {
        var pool = GetPool(obj.Type);

        obj.Field = null;

        if (obj.FieldSplit != null)
        {
            if (obj is IFieldSplitObserver observer)
                foreach (var split in observer.Observing.ToImmutableList())
                    await split.Unobserve(observer, true);
            await obj.FieldSplit.Leave(obj, getLeavePacket: getLeavePacket);
        }

        if (pool != null) await pool.Leave(obj);

        if (obj is IFieldUser owner)
            foreach (var owned in owner.Owned)
                await Leave(owned);
    }

    public override IFieldObject? GetObject(int id) => Objects.FirstOrDefault(o => o.ObjectID == id);
    public T? GetObject<T>(int id) where T : IFieldObject => Objects.OfType<T>().FirstOrDefault(o => o.ObjectID == id);


    private IFieldSplit?[] GetEnclosingSplits(int row, int col)
    {
        var splits = new IFieldSplit?[9];

        splits[0] = GetSplit(row - 1, col - 1);
        splits[1] = GetSplit(row - 1, col);
        splits[2] = GetSplit(row - 1, col + 1);

        splits[3] = GetSplit(row, col - 1);
        splits[4] = GetSplit(row, col);
        splits[5] = GetSplit(row, col + 1);

        splits[6] = GetSplit(row + 1, col - 1);
        splits[7] = GetSplit(row + 1, col);
        splits[8] = GetSplit(row + 1, col + 1);

        return splits;
    }

    private IFieldSplit? GetSplit(int row, int col)
    {
        if (
            row < 0 || row >= _splits.GetLength(0) ||
            col < 0 || col >= _splits.GetLength(1)
        ) return null;
        return _splits[row, col];
    }
    
    public async Task OnTick(DateTime now)
    {
        if (GetPool(FieldObjectType.User)?.Objects.Count == 0) return;

        await Task.WhenAll(
            Objects
                .OfType<ITickable>()
                .Select(o => o.OnTick(now))
        );

        if (now > NextGeneratorTick)
        {
            NextGeneratorTick = now.AddSeconds(7);
            await Task.WhenAll((await Generators.RetrieveAll())
                .Select(g => g.Generate()));
        }
    }
}
