using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using Edelstein.Provider.Templates.Field.Life;
using Edelstein.Provider.Templates.Field.Reactor;
using MoreLinq.Extensions;

namespace Edelstein.Provider.Templates.Field
{
    public class FieldTemplate : ITemplate
    {
        public int ID { get; set; }

        public FieldOpt Limit { get; set; }
        public Rectangle Bounds { get; set; }

        public int? FieldReturn { get; set; }
        public int? ForcedReturn { get; set; }

        public string? ScriptFirstUserEnter { get; set; }
        public string? ScriptUserEnter { get; set; }

        public IDictionary<int, FieldFootholdTemplate> Footholds { get; }
        public IDictionary<int, FieldPortalTemplate> Portals { get; }
        public ICollection<FieldLifeTemplate> Life { get; }
        public ICollection<FieldReactorTemplate> Reactors { get; }

        public double MobRate { get; set; }
        public int MobCapacityMin { get; set; }
        public int MobCapacityMax { get; set; }

        public FieldTemplate(int id, IDataProperty property)
        {
            ID = id;

            Footholds = property.Resolve("foothold").Children
                .SelectMany(c => c.Children)
                .SelectMany(c => c.Children)
                .Select(p => new FieldFootholdTemplate(p.ResolveAll()))
                .DistinctBy(x => x.ID) // 211040101 has duplicate footholds
                .ToDictionary(x => x.ID, x => x);
            Portals = property.Resolve("portal").Children
                .Select(p => new FieldPortalTemplate(p.ResolveAll()))
                .DistinctBy(x => x.ID)
                .ToDictionary(x => x.ID, x => x);
            Life = property.Resolve("life").Children
                .Select(p => new FieldLifeTemplate(p.ResolveAll()))
                .ToList();
            Reactors = property.Resolve("reactor")?.Children
                           .Select(p => new FieldReactorTemplate(p.ResolveAll()))
                           .ToList()
                       ?? new List<FieldReactorTemplate>();

            property.Resolve("info").ResolveAll(i =>
            {
                Limit = (FieldOpt) (i.Resolve<int>("fieldLimit") ?? 0);

                FieldReturn = i.Resolve<int>("returnMap");
                ForcedReturn = i.Resolve<int>("forcedReturn");
                if (FieldReturn == 999999999) FieldReturn = null;
                if (ForcedReturn == 999999999) ForcedReturn = null;

                ScriptFirstUserEnter = i.ResolveOrDefault<string>("onFirstUserEnter");
                ScriptUserEnter = i.ResolveOrDefault<string>("onUserEnter");
                if (string.IsNullOrWhiteSpace(ScriptFirstUserEnter)) ScriptFirstUserEnter = null;
                if (string.IsNullOrWhiteSpace(ScriptUserEnter)) ScriptUserEnter = null;


                var footholds = Footholds.Values;
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

                MobRate = i.Resolve<double>("mobRate") ?? 1.0;
                Bounds = Rectangle.FromLTRB(leftTop.X, leftTop.Y, rightBottom.X, rightBottom.Y);

                var mobCapacity = Bounds.Size.Height * Bounds.Size.Width * MobRate * 0.0000078125;

                mobCapacity = Math.Min(mobCapacity, 40);
                mobCapacity = Math.Max(mobCapacity, 1);

                MobCapacityMin = (int) mobCapacity;
                MobCapacityMax = (int) mobCapacity * 2;
            });
        }
    }
}