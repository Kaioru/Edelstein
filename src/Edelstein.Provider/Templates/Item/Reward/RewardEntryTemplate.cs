namespace Edelstein.Provider.Templates.Item.Reward
{
    public class RewardEntryTemplate : ITemplate
    {
        public int ID { get; }

        public int TemplateID { get; }
        public int MinQuantity { get; }
        public int MaxQuantity { get; }

        public int Money { get; }

        public float Prob { get; }

        public RewardEntryTemplate(int id, IDataProperty property)
        {
            ID = id;

            TemplateID = property.Resolve<int>("item") ?? 0;
            MinQuantity = property.Resolve<int>("min") ?? 1;
            MaxQuantity = property.Resolve<int>("max") ?? 1;

            Money = property.Resolve<int>("money") ?? 0;

            Prob = property.Resolve<float>("prob") ?? 0.0f;
        }
    }
}