using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Edelstein.Provider;
using MoreLinq;

namespace Edelstein.Core.Templates.Etc.MakeCharInfo
{
    public class MakeCharInfoTemplateCollection : AbstractEagerDataTemplateCollection
    {
        private IDataDirectoryCollection _collection;

        public MakeCharInfoTemplateCollection(IDataDirectoryCollection collection)
            => _collection = collection;

        public override async Task Populate()
        {
            var property = _collection.Resolve("Etc/MakeCharInfo.img").ResolveAll();

            new List<Tuple<MakeCharInfoType, int, string>>
                {
                    Tuple.Create(MakeCharInfoType.Normal, 0, "Info/CharMale"),
                    Tuple.Create(MakeCharInfoType.Normal, 1, "Info/CharFemale"),
                    Tuple.Create(MakeCharInfoType.Premium, 0, "PremiumCharMale"),
                    Tuple.Create(MakeCharInfoType.Premium, 1, "PremiumCharFemale"),
                    Tuple.Create(MakeCharInfoType.Orient, 0, "OrientCharMale"),
                    Tuple.Create(MakeCharInfoType.Orient, 1, "OrientCharFemale"),
                    Tuple.Create(MakeCharInfoType.Evan, 0, "EvanCharMale"),
                    Tuple.Create(MakeCharInfoType.Evan, 1, "EvanCharFemale"),
                    Tuple.Create(MakeCharInfoType.Resistance, 0, "ResistanceCharMale"),
                    Tuple.Create(MakeCharInfoType.Resistance, 1, "ResistanceCharFemale"),
                }
                .Select(t => new MakeCharInfoTemplate(
                    t.Item1,
                    (byte) t.Item2,
                    property.Resolve(t.Item3).ResolveAll()
                ))
                .ForEach(t => Templates.Add(t.ID, t));
        }
    }
}