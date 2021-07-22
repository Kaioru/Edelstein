using System;
using System.Threading.Tasks;
using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Common.Gameplay.Templating
{
    public class TemplateProvider<TEntry> where TEntry : class, ITemplate
    {
        public int ID { get; }
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
