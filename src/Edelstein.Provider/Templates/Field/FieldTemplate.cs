using System.Collections.Generic;
using System.Drawing;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplate : ITemplate
    {
        public int ID { get; set; }

        public Rectangle Bounds { get; set; }
        public Size Size { get; set; }

        public IDictionary<int, FieldFootholdTemplate> Footholds;
        public IDictionary<int, FieldPortalTemplate> Portals;
        public ICollection<FieldLifeTemplate> Life;
        public ICollection<FieldReactorTemplate> Reactors;
    }
}