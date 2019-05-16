namespace Edelstein.Provider.Templates.String
{
    public class SkillStringTemplate : IStringTemplate
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Desc { get; set; }
        
        public SkillStringTemplate(int id, IDataProperty property)
        {
            ID = id;
            
            Name = property.ResolveOrDefault<string>("name") ?? "NO-NAME";
            Desc = property.ResolveOrDefault<string>("desc");
        }
    }
}