using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Game.Objects.Mob.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Game.Objects.Mob.Templates;

public record MobStringTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<IMobStringTemplate> _manager;

    public MobStringTemplateLoader(IDataNamespace data, ITemplateManager<IMobStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("String/Mob.img")?.Children
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<IMobStringTemplate>(
                    id,
                    new MobStringTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }) ?? Array.Empty<Task>());
        
        _manager.Freeze();
        return _manager.Count;
    }
}
