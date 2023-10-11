using Duey.Abstractions;
using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillStringTemplateLoader : ITemplateLoader
{
    private readonly IDataNamespace _data;
    private readonly ITemplateManager<ISkillStringTemplate> _manager;
    
    public SkillStringTemplateLoader(IDataNamespace data, ITemplateManager<ISkillStringTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        await Task.WhenAll(_data.ResolvePath("String/Skill.img")?.Children
            .Where(c => c.Name.Length > 3)
            .Where(c => c.Name.All(char.IsDigit))
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderEager<ISkillStringTemplate>(
                    id,
                    new SkillStringTemplate(
                        id,
                        n.Cache()
                    )
                ));
            }) ?? Array.Empty<Task>());

        return _manager.Count;
    }
}
