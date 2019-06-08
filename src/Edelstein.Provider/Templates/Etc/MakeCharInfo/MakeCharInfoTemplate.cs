using System.Linq;

namespace Edelstein.Provider.Templates.Etc.MakeCharInfo
{
    public class MakeCharInfoTemplate : ITemplate
    {
        public int ID { get; }

        public MakeCharInfoType Type { get; }
        public byte Gender { get; }

        public int[] Face { get; }
        public int[] Hair { get; }
        public int[] HairColor { get; }
        public int[] Skin { get; }
        public int[] Coat { get; }
        public int[] Pants { get; }
        public int[] Shoes { get; }
        public int[] Weapon { get; }

        public MakeCharInfoTemplate(MakeCharInfoType type, byte gender, IDataProperty property)
        {
            ID = (int) type * 0x2 + gender;
            Type = type;
            Gender = gender;

            Face = property.Resolve("0").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Hair = property.Resolve("1").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            HairColor = property.Resolve("2").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Skin = property.Resolve("3").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Coat = property.Resolve("4").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Pants = property.Resolve("5").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Shoes = property.Resolve("6").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
            Weapon = property.Resolve("7").Children.Select(c => c.Resolve<int>() ?? 0).ToArray();
        }
    }
}