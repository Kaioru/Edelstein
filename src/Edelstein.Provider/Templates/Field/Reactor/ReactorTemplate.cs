namespace Edelstein.Provider.Templates.Field.Reactor
{
    public class ReactorTemplate : ITemplate
    {
        public int ID { get; }

        public ReactorTemplate(int id, IDataProperty property)
        {
            ID = id;
        }
    }
}