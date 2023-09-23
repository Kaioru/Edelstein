using System.Globalization;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public class ModifiedQuestTimeTemplate : ITemplate, IModifiedQuestTime
{
    public int ID { get; }
    public DateTime DateStart { get; }
    public DateTime DateEnd { get; }

    public ModifiedQuestTimeTemplate(int id, IDataProperty property)
    {
        ID = id;

        var start = property.ResolveOrDefault<string>("start");
        var end = property.ResolveOrDefault<string>("end");

        if (start != null) DateStart = DateTime.ParseExact(start[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
        if (end != null) DateEnd = DateTime.ParseExact(end[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
    }
}
