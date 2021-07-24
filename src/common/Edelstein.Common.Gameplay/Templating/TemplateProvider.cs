using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Util.Repositories;

namespace Edelstein.Common.Gameplay.Templating
{
    public class TemplateProvider<TEntry> : IRepositoryEntry<int> where TEntry : class, ITemplate
    {
        public int ID { get; set; }
        private readonly Func<TEntry> _func;

        public TemplateProvider(int id, Func<TEntry> func)
        {
            ID = id;
            _func = func;
        }

        public Task<TEntry> Provide()
            => Task.FromResult(_func.Invoke());
    }
}
