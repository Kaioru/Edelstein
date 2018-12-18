using System.Drawing;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider.Parser;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplateCollection : AbstractLazyTemplateCollection
    {
        public FieldTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        public override Task<ITemplate> Load(int id)
        {
            var p = Collection.Resolve($"Map/Map/Map{id.ToString("D8")[0]}/{id:D8}.img");
            p = p ?? Collection.Resolve($"Map/Map/Map{id.ToString("D9")[0]}/{id:D9}.img");

            var link = p.Resolve<int>("info/link");
            if (link.HasValue) Load(link.Value);

            var res = new FieldTemplate
            {
                ID = id,
                Limit = (FieldOpt) (p.Resolve<int>("fieldLimit") ?? 0),
                Footholds = p.Resolve("foothold").Children
                    .SelectMany(c => c.Children)
                    .SelectMany(c => c.Children)
                    .Select(FieldFootholdTemplate.Parse)
                    .DistinctBy(x => x.ID) // 211040101 has duplicate footholds
                    .ToDictionary(x => x.ID, x => x),
                Portals = p.Resolve("portal").Children
                    .Select(FieldPortalTemplate.Parse)
                    .DistinctBy(x => x.ID)
                    .ToDictionary(x => x.ID, x => x),
                Life = p.Resolve("life").Children
                    .Select(FieldLifeTemplate.Parse)
                    .ToList(),
                Reactors = p.Resolve("reactor").Children
                    .Select(FieldReactorTemplate.Parse)
                    .ToList()
            };

            var footholds = res.Footholds.Values;
            var leftTop = new Point(
                footholds.Select(f => f.X1 > f.X2 ? f.X2 : f.X1).OrderBy(f => f).First(),
                footholds.Select(f => f.Y1 > f.Y2 ? f.Y2 : f.Y1).OrderBy(f => f).First()
            );
            var rightBottom = new Point(
                footholds.Select(f => f.X1 > f.X2 ? f.X1 : f.X2).OrderByDescending(f => f).First(),
                footholds.Select(f => f.Y1 > f.Y2 ? f.Y1 : f.Y2).OrderByDescending(f => f).First()
            );

            leftTop = new Point(
                p.Resolve<int>("info/VRLeft") ?? leftTop.X,
                p.Resolve<int>("info/VRTop") ?? leftTop.Y
            );
            rightBottom = new Point(
                p.Resolve<int>("info/VRRight") ?? rightBottom.X,
                p.Resolve<int>("info/VRBottom") ?? rightBottom.Y
            );

            res.Bounds = Rectangle.FromLTRB(leftTop.X, leftTop.Y, rightBottom.X, rightBottom.Y);
            return Task.FromResult<ITemplate>(res);
        }
    }
}