using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Core.Provider;
using Edelstein.Core.Templates.Server.Continent;
using Edelstein.Core.Utils.Ticks;

namespace Edelstein.Service.Game.Fields.Continent
{
    public class ContinentManager : ITickBehavior
    {
        private readonly ICollection<Continent> _continents;

        public ContinentManager(IDataTemplateManager templateManager, FieldManager fieldManager)
        {
            _continents = templateManager
                .GetAll<ContinentTemplate>()
                .Select(t => new Continent(t, fieldManager))
                .ToList();
        }

        public ICollection<Continent> All()
            => _continents.ToImmutableList();

        public Continent Get(int id)
            => _continents.FirstOrDefault(c => c.Template.ID == id);

        public Task TryTick(DateTime now)
            => Task.WhenAll(_continents.Select(c => c.TryTick(now)));
    }
}