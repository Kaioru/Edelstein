using System.Drawing;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Field.Continent
{
    public class ContinentGenMobTemplate : ITemplate
    {
        public int ID { get; }

        public int TemplateID { get; set; }
        public Point Position { get; set; }

        public ContinentGenMobTemplate(int id, IDataProperty property)
        {
            ID = id;

            TemplateID = property.Resolve<int>("genMobItemID") ?? 0;
            Position = new Point(
                property.Resolve<int>("genMobPosition_x") ?? 0,
                property.Resolve<int>("genMobPosition_y") ?? 0
            );
        }
    }
}