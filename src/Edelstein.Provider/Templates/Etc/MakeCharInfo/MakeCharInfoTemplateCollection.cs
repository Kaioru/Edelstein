using System.Collections.Generic;

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
                    property.Resolve("Info/CharMale")
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Normal,
                    1,
                    property.Resolve("Info/CharFemale")
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Premium,
                    0,
                    property.Resolve("PremiumCharMale")
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Premium,
                    1,
                    property.Resolve("PremiumCharFemale")
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Orient,
                    0,
                    property.Resolve("OrientCharMale")
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Orient,
                    1,
                    property.Resolve("OrientCharFemale")
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Evan,
                    0,
                    property.Resolve("EvanCharMale")
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Evan,
                    1,
                    property.Resolve("EvanCharFemale")
                ),

                new MakeCharInfoTemplate(
                    MakeCharInfoType.Resistance,
                    0,
                    property.Resolve("ResistanceCharMale")
                ),
                new MakeCharInfoTemplate(
                    MakeCharInfoType.Resistance,
                    1,
                    property.Resolve("ResistanceCharFemale")
                ),
            };
        }
    }
}