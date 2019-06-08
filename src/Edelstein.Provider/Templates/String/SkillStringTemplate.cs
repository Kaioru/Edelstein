namespace Edelstein.Provider.Templates.String
{
    public class SkillStringTemplate : IStringTemplate
    {
        public int ID { get; }
        public string Name { get; }
        public string Desc { get; }
        
        public SkillStringTemplate(int id, IDataProperty property)
        {
            ID = id;
            
            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
            Desc = property.ResolveOrDefault<string>("desc");
        }
    }
}