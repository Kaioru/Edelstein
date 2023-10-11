using System.Globalization;
using Duey.Abstractions;
using Edelstein.Protocol.Gameplay.Game.Quests;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Quests.Templates;

public class ModifiedQuestTimeTemplate : ITemplate, IModifiedQuestTime
{
    public int ID { get; }
    public DateTime DateStart { get; }
    public DateTime DateEnd { get; }

    public ModifiedQuestTimeTemplate(int id, IDataNode node)
    {
        ID = id;

        var start = node.ResolveString("start");
        var end = node.ResolveString("end");

        if (start != null) DateStart = DateTime.ParseExact(start[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
        if (end != null) DateEnd = DateTime.ParseExact(end[..10], "yyyyMMddHH", CultureInfo.InvariantCulture);
    }
}
