using System;
using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Common.Gameplay.Templating
{
    public class TemplateRepositoryEntry<TEntry> : AbstractTemplateRepositoryEntry<TEntry> where TEntry : class, ITemplate
    {
        private readonly Func<TEntry> _func;

        public TemplateRepositoryEntry(int id, TimeSpan duration, Func<TEntry> func) : base(id, duration)
            => _func = func;

        public override TEntry Load()
            => _func.Invoke();
    }
}
