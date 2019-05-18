using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Utils;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Field.Continent;

namespace Edelstein.Service.Game.Fields.Continents
{
    public class ContinentManager : ITickable
    {
        private DateTime LastUpdate { get; set; }
        public ICollection<Continent> Continents { get; }

        public ContinentManager(ITemplateManager templateManager, FieldManager fieldManager)
        {
            Continents = templateManager
                .GetAll<ContinentTemplate>()
                .Select(c => new Continent(fieldManager, c))
                .ToList();
        }

        public Continent Get(int id)
            => Continents.FirstOrDefault(c => c.Template.ID == id);

        public async Task OnTick(DateTime now)
        {
            if ((now - LastUpdate).TotalSeconds < 30) return;

            LastUpdate = now;
            await Task.WhenAll(Continents.Select(c => c.OnTick(now)));
        }
    }
}