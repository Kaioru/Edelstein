using System;
using Edelstein.Protocol.Gameplay.Templating;

namespace Edelstein.Common.Gameplay.Templating
{
    public abstract class AbstractTemplateRepositoryEntry<TTemplate> where TTemplate : class, ITemplate
    {
        public int ID { get; init; }
        public TimeSpan Duration { get; init; }

        public TTemplate Template
        {
            get
            {
                if (_template == null)
                {
                    _template = Load();
                    DateLoaded = DateTime.UtcNow;
                }

                DateExpire = DateTime.UtcNow.Add(Duration);
                return _template;
            }
            set
            {
                if (value != null) DateExpire = DateTime.UtcNow.Add(Duration);
                _template = value;
            }
        }
        public DateTime DateLoaded { get; private set; }
        public DateTime DateExpire { get; private set; }

        private TTemplate _template;

        protected AbstractTemplateRepositoryEntry(int id, TimeSpan duration)
        {
            ID = id;
            Duration = duration;
        }

        public void Reset()
            => _template = null;

        public abstract TTemplate Load();
    }
}
