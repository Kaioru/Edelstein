using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Services;
using Edelstein.Provider.Templates;
using Edelstein.Provider.Templates.Server.Continent;

namespace Edelstein.Service.Game.Fields.Continent
{
    public class ContinentManager : IUpdateable
    {
        private readonly ITemplateManager _templateManager;
        private readonly FieldManager _fieldManager;
        private readonly ICollection<Continent> _continents;

        private DateTime LastUpdate { get; set; }

        public ContinentManager(ITemplateManager templateManager, FieldManager fieldManager)
        {
            _templateManager = templateManager;
            _fieldManager = fieldManager;

            _continents = _templateManager
                .GetAll<ContinentTemplate>()
                .Select(c => new Continent(c, _fieldManager))
                .ToList();
        }

        public async Task OnUpdate(DateTime now)
        {
            if ((now - LastUpdate).Seconds < 30) return;

            LastUpdate = now;
            await Task.WhenAll(_continents.Select(c => c.OnUpdate(now)));
        }
    }
}