using System;
using System.Linq;
using System.Threading.Tasks;
using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.NPC.Templates;
using Edelstein.Protocol.Utilities.Templates;
using Microsoft.Extensions.Logging;

namespace Edelstein.Common.Gameplay.Game.Objects.NPC.Templates;

public class NPCTemplateLoader(
    ILogger<AbstractTemplateLoader<INPCTemplate>> logger, 
    ITemplateManagerContext<INPCTemplate> context,
    IDataNamespace data
) : AbstractTemplateLoader<INPCTemplate>(logger, context)
{
    protected override async Task Load(ITemplateManagerContext<INPCTemplate> context)
    {
        var directory = data.ResolvePath("Npc");

        if (directory == null) return;

        await Task.WhenAll(directory.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await context.Insert(new TemplateProviderLazy<INPCTemplate>(
                    id,
                    () =>
                    {
                        var node = n.Cache();
                        return new NPCTemplate(
                            id,
                            node,
                            node.ResolvePath("info")!.Cache()
                        );
                    }));
            }));
    }
}
