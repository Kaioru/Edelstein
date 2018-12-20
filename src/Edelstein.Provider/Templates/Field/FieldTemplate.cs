using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplate : ITemplate
    {
        public int ID { get; set; }

        public FieldOpt Limit { get; set; }

        public Rectangle Bounds { get; set; }
        public Size Size { get; set; }

        public IDictionary<int, FieldFootholdTemplate> Footholds;
        public IDictionary<int, FieldPortalTemplate> Portals;
        public ICollection<FieldLifeTemplate> Life;
        public ICollection<FieldReactorTemplate> Reactors;


        public static FieldTemplate Parse(int id, IDataProperty property)
        {
            var t = new FieldTemplate {ID = id};

            property.Resolve(p =>
            {
                t.Footholds = p.Resolve("foothold").Children
                    .SelectMany(c => c.Children)
                    .SelectMany(c => c.Children)
                    .Select(FieldFootholdTemplate.Parse)
                    .DistinctBy(x => x.ID) // 211040101 has duplicate footholds
                    .ToDictionary(x => x.ID, x => x);
                t.Portals = p.Resolve("portal").Children
                    .Select(FieldPortalTemplate.Parse)
                    .DistinctBy(x => x.ID)
                    .ToDictionary(x => x.ID, x => x);
                t.Life = p.Resolve("life").Children
                    .Select(FieldLifeTemplate.Parse)
                    .ToList();
                t.Reactors = p.Resolve("reactor").Children
                    .Select(FieldReactorTemplate.Parse)
                    .ToList();

                p.Resolve("info").Resolve(i =>
                {
                    t.Limit = (FieldOpt) (i.Resolve<int>("fieldLimit") ?? 0);

                    var footholds = t.Footholds.Values;
                    var leftTop = new Point(
                        footholds.Select(f => f.X1 > f.X2 ? f.X2 : f.X1).OrderBy(f => f).First(),
                        footholds.Select(f => f.Y1 > f.Y2 ? f.Y2 : f.Y1).OrderBy(f => f).First()
                    );
                    var rightBottom = new Point(
                        footholds.Select(f => f.X1 > f.X2 ? f.X1 : f.X2).OrderByDescending(f => f).First(),
                        footholds.Select(f => f.Y1 > f.Y2 ? f.Y1 : f.Y2).OrderByDescending(f => f).First()
                    );

                    leftTop = new Point(
                        i.Resolve<int>("VRLeft") ?? leftTop.X,
                        i.Resolve<int>("VRTop") ?? leftTop.Y
                    );
                    rightBottom = new Point(
                        i.Resolve<int>("VRRight") ?? rightBottom.X,
                        i.Resolve<int>("VRBottom") ?? rightBottom.Y
                    );

                    t.Bounds = Rectangle.FromLTRB(leftTop.X, leftTop.Y, rightBottom.X, rightBottom.Y);
                });
            });
            return t;
        }
    }
}