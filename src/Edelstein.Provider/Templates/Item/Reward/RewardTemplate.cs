namespace Edelstein.Provider.Templates.Item.Reward
{
    public class RewardTemplate : ITemplate
    {
        public int ID { get; }

        public int TemplateID { get; }
        public int MinQuantity { get; }
        public int MaxQuantity { get; }

        public int Money { get; }

        public float Prob { get; }

        public RewardTemplate(int id, IDataProperty property)
        {
            ID = id;

            TemplateID = property.Resolve<int>("item") ?? 0;
            MinQuantity = property.Resolve<int>("min") ?? 0;
            MaxQuantity = property.Resolve<int>("max") ?? 0;

            Money = property.Resolve<int>("money") ?? 0;

            Prob = property.Resolve<float>("prob") ?? 0.0f;
        }
    }
}