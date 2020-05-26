using System.Drawing;
using Edelstein.Core.Provider;

namespace Edelstein.Core.Templates.Server.Continent
{
    public class ContinentGenMobTemplate
    {
        public int TemplateID { get; set; }
        public Point Position { get; set; }

        public ContinentGenMobTemplate(IDataProperty property)
        {
            TemplateID = property.Resolve<int>("genMobItemID") ?? 0;
            Position = new Point(
                property.Resolve<int>("genMobPosition_x") ?? 0,
                property.Resolve<int>("genMobPosition_y") ?? 0
            );
        }
    }
}