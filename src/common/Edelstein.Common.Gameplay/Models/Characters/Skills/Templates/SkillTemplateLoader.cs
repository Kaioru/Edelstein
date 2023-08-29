using Edelstein.Common.Utilities.Templates;
using Edelstein.Protocol.Data;
using Edelstein.Protocol.Gameplay.Models.Characters.Skills.Templates;
using Edelstein.Protocol.Utilities.Templates;

namespace Edelstein.Common.Gameplay.Models.Characters.Skills.Templates;

public class SkillTemplateLoader : ITemplateLoader
{
    private readonly IDataManager _data;
    private readonly ITemplateManager<ISkillTemplate> _manager;
    
    public SkillTemplateLoader(IDataManager data, ITemplateManager<ISkillTemplate> manager)
    {
        _data = data;
        _manager = manager;
    }

    public async Task<int> Load()
    {
        var dirSkills = _data.Resolve("Skill")?.ResolveAll();

        if (dirSkills == null) return 0;
        
        await Task.WhenAll(dirSkills.Children
            .Where(c => c.Name.Split(".")[0].All(char.IsDigit))
            .Where(c => c.Resolve("skill") != null)
            .SelectMany(c => c.Resolve("skill")!.Children)
            .Select(async n =>
            {
                var id = Convert.ToInt32(n.Name);
                await _manager.Insert(new TemplateProviderLazy<ISkillTemplate>(
                    id,
                    () => new SkillTemplate(id, n.ResolveAll())
                ));
            }));
        
        return _manager.Count;
    }
}
