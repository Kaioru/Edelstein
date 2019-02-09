using System.Collections.Generic;
using System.Linq;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Server.FieldSet;

namespace Edelstein.Service.Game.Fields
{
    public class FieldSetManager
    {
        public IDictionary<string, FieldSet> FieldSets { get; }

        public FieldSetManager(ITemplateManager templateManager, FieldManager fieldManager)
        {
            FieldSets = templateManager
                .GetAll<FieldSetTemplate>()
                .Select(f => new FieldSet(f, fieldManager))
                .ToDictionary(
                    f => f.SetTemplate.Name,
                    f => f
                );
        }

        public IFieldSet Get(string name)
            => FieldSets.ContainsKey(name) ? FieldSets[name] : null;
    }
}