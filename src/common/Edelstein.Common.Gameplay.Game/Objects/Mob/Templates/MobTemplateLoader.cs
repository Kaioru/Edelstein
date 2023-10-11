﻿using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public class MobTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IMobTemplate> _manager;

    public MobTemplateLoader(IDataNamespace data, ITemplateManager<IMobTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var directory = _data.ResolvePath("Mob")?.ResolveAll();

        if (directory == null) return 0;

        await Task.WhenAll(directory.Children
            .Where(n => n.Name.Split(".")[0].All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name.Split(".")[0]);
                await _manager.Insert(new TemplateProviderLazy<IMobTemplate>(
                    id,
                    () => new MobTemplate(
                        id,
                        n.ResolveAll(),
                        n.ResolvePath("info")!.ResolveAll()
                    )
                ));
            }));

        return _manager.Count;
    }
}
