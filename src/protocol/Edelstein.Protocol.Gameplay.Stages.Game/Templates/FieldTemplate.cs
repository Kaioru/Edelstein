using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Edelstein.Protocol.Gameplay.Templating;
using Edelstein.Protocol.Parser;
using Edelstein.Protocol.Util.Spatial;
using MoreLinq;

namespace Edelstein.Protocol.Gameplay.Stages.Game.Templates
{
    public record FieldTemplate : ITemplate
    {
        public int ID { get; init; }

        public FieldOpt Limit { get; init; }
        public Rect2D Bounds { get; init; }

        public int? FieldReturn { get; init; }
        public int? ForcedReturn { get; init; }

        public string ScriptFirstUserEnter { get; init; }
        public string ScriptUserEnter { get; init; }

        public IDictionary<int, FieldPortalTemplate> Portals { get; }
        public IDictionary<int, FieldFootholdTemplate> Footholds { get; }
        public IDictionary<int, FieldLadderOrRopeTemplate> LadderOrRopes { get; }

        public ICollection<FieldLifeTemplate> Life { get; }
        //public ICollection<FieldReactorTemplate> Reactors { get; }

        public double MobRate { get; init; }
        public int MobCapacityMin { get; init; }
        public int MobCapacityMax { get; init; }

        public FieldTemplate(
            int id,
            IDataProperty foothold,
            IDataProperty portal,
            IDataProperty ladderRope,
            IDataProperty life,
            IDataProperty info
        )
        {
            ID = id;

            Footholds = foothold.Children
                .SelectMany(c => c.Children)
                .SelectMany(c => c.Children)
                .Select(p => Tuple.Create(
                    Convert.ToInt32(p.Name),
                    new FieldFootholdTemplate(Convert.ToInt32(p.Name), p.ResolveAll())
                ))
                .DistinctBy(t => t.Item1) // 211040101 has duplicate footholds
                .ToImmutableDictionary(
                    t => Convert.ToInt32(t.Item1),
                    t => t.Item2
                );
            Portals = portal.Children
                .Select(p => Tuple.Create(
                    Convert.ToInt32(p.Name),
                    new FieldPortalTemplate(Convert.ToInt32(p.Name), p.ResolveAll())
                ))
                .DistinctBy(t => t.Item1)
                .ToImmutableDictionary(
                    t => t.Item1,
                    t => t.Item2
                );
            LadderOrRopes = ladderRope.Children
                .Select(p => Tuple.Create(
                    Convert.ToInt32(p.Name),
                    new FieldLadderOrRopeTemplate(Convert.ToInt32(p.Name), p.ResolveAll())
                ))
                .DistinctBy(t => t.Item1)
                .ToImmutableDictionary(
                    t => t.Item1,
                    t => t.Item2
                );

            Life = life.Children
                .Select(p => new FieldLifeTemplate(Convert.ToInt32(p.Name), p.ResolveAll()))
                .ToImmutableList();

            Limit = (FieldOpt)(info.Resolve<int>("fieldLimit") ?? 0);

            FieldReturn = info.Resolve<int>("returnMap");
            ForcedReturn = info.Resolve<int>("forcedReturn");
            if (FieldReturn == 999999999) FieldReturn = null;
            if (ForcedReturn == 999999999) ForcedReturn = null;

            ScriptFirstUserEnter = info.ResolveOrDefault<string>("onFirstUserEnter");
            ScriptUserEnter = info.ResolveOrDefault<string>("onUserEnter");
            if (string.IsNullOrWhiteSpace(ScriptFirstUserEnter)) ScriptFirstUserEnter = null;
            if (string.IsNullOrWhiteSpace(ScriptUserEnter)) ScriptUserEnter = null;


            var footholds = Footholds.Values;
            var leftTop = new Point2D(
                footholds.SelectMany(f => new List<int>() { f.Line.Start.X, f.Line.End.X }).OrderBy(f => f).First(),
                footholds.SelectMany(f => new List<int>() { f.Line.Start.Y, f.Line.End.Y }).OrderBy(f => f).First()
            );
            var rightBottom = new Point2D(
                footholds.SelectMany(f => new List<int>() { f.Line.Start.X, f.Line.End.X }).OrderByDescending(f => f).First(),
                footholds.SelectMany(f => new List<int>() { f.Line.Start.Y, f.Line.End.Y }).OrderByDescending(f => f).First()
            );

            leftTop = new Point2D(
                info.Resolve<int>("VRLeft") ?? leftTop.X,
                info.Resolve<int>("VRTop") ?? leftTop.Y
            );
            rightBottom = new Point2D(
                info.Resolve<int>("VRRight") ?? rightBottom.X,
                info.Resolve<int>("VRBottom") ?? rightBottom.Y
            );

            MobRate = info.Resolve<double>("mobRate") ?? 1.0;
            Bounds = new Rect2D(leftTop, rightBottom);

            var mobCapacity = Bounds.Size.Height * Bounds.Size.Width * MobRate * 0.0000078125;

            mobCapacity = Math.Min(mobCapacity, 40);
            mobCapacity = Math.Max(mobCapacity, 1);

            MobCapacityMin = (int)mobCapacity;
            MobCapacityMax = (int)mobCapacity * 2;
        }
    }
}
