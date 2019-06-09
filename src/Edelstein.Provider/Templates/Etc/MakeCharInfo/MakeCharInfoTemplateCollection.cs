using System.Collections.Generic;
using Edelstein.Provider.Parsing;

namespace Edelstein.Provider.Templates.Etc.MakeCharInfo
{
    public class MakeCharInfoTemplateCollection : AbstractEagerTemplateCollection
    {
        public override TemplateCollectionType Type => TemplateCollectionType.MakeCharInfo;

        public MakeCharInfoTemplateCollection(IDataDirectoryCollection collection) : base(collection)
        {
        }

        protected override IEnumerable<ITemplate> Load()
        {
            var property = Collection.Resolve("Etc/MakeCharInfo.img").ResolveAll();

            return new List<ITemplate>
            {
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Normal,
                    0,
                    property.Resolve("Info/CharMale").ResolveAll()
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Normal,
                    1,
                    property.Resolve("Info/CharFemale").ResolveAll()
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Premium,
                    0,
                    property.Resolve("PremiumCharMale").ResolveAll()
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Premium,
                    1,
                    property.Resolve("PremiumCharFemale").ResolveAll()
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Orient,
                    0,
                    property.Resolve("OrientCharMale").ResolveAll()
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Orient,
                    1,
                    property.Resolve("OrientCharFemale").ResolveAll()
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Evan,
                    0,
                    property.Resolve("EvanCharMale").ResolveAll()
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Evan,
                    1,
                    property.Resolve("EvanCharFemale").ResolveAll()
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Resistance,
                    0,
                    property.Resolve("ResistanceCharMale").ResolveAll()
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Resistance,
                    1,
                    property.Resolve("ResistanceCharFemale").ResolveAll()
                ),
            };
        }
    }
}