namespace Edelstein.Provider.Templates.Item.Consume
{
    public class PortalScrollItemTemplate : ItemBundleTemplate
    {
        public int MoveTo { get; set; }
        
        // Continent
        // FieldSet
        // QR
        
        public PortalScrollItemTemplate(int id, IDataProperty info, IDataProperty spec) : base(id, info)
        {
            MoveTo = spec.Resolve<short>("spec/moveTo") ?? 0;
        }
    }
}