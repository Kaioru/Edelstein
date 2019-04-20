using System.Collections.Generic;

namespace Edelstein.Provider.Templates
{
    public interface ITemplateManager
    {
        T Get<T>(int id) where T : ITemplate;
        IEnumerable<T> GetAll<T>() where T : ITemplate;
    }
}