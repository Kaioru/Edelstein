using System;
using System.Threading.Tasks;

namespace Edelstein.Service.Game.Fields.Generators
{
    public abstract class AbstractFieldGenerator : IFieldGenerator
    {
        public abstract TimeSpan RegenInterval { get; }
        public DateTime RegenAfter { get; private set; }
        public IFieldGeneratedObj Generated { get; protected set; }

        public Task Reset()
        {
            var interval = TimeSpan.Zero;

            if (RegenInterval.Seconds > 0)
            {
                var random = new Random();
                var buffer = 7 * RegenInterval.Seconds / 10;

                interval = TimeSpan.FromSeconds(
                    13 * RegenInterval.Seconds / 10 + random.Next(buffer)
                );
            }

            Generated = null;
            RegenAfter = DateTime.Now.Add(interval);
            return Task.CompletedTask;
        }

        public abstract Task Generate(IField field);
    }
}