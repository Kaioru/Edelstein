using System.Linq;

namespace Edelstein.Provider.Templates.Etc.MakeCharInfo
{
    public class MakeCharInfoTemplate : ITemplate
    {
        public int ID { get; set; }

        public MakeCharInfoType Type { get; set; }
        public byte Gender { get; set; }

        public int[] Face { get; set; }
        public int[] Hair { get; set; }
        public int[] HairColor { get; set; }
        public int[] Skin { get; set; }
        public int[] Coat { get; set; }
        public int[] Pants { get; set; }
        public int[] Shoes { get; set; }
        public int[] Weapon { get; set; }

        public MakeCharInfoTemplate(MakeCharInfoType type, byte gender, IDataProperty property)
        {
            ID = (int) type * 0x2 + gender;
            Type = type;
            Gender = gender;

            property.ResolveAll(p =>
            {
                Face = p.Resolve("0").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Hair = p.Resolve("1").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                HairColor = p.Resolve("2").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Skin = p.Resolve("3").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Coat = p.Resolve("4").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Pants = p.Resolve("5").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Shoes = p.Resolve("6").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
                Weapon = p.Resolve("7").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            });
        }
    }
}