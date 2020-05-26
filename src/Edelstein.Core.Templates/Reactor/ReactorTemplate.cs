using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Reactor
{
    public class ReactorTemplate : IDataTemplate
    {
        public int ID { get; }

        public ReactorTemplate(int id, IDataProperty property)
        {
            ID = id;
        }
    }
}