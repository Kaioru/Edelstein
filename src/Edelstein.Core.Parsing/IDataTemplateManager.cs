using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Edelstein.Core.Provider
{
    public interface IDataTemplateManager
    {
        Task Register<T>(IDataTemplateCollection collection) where T : IDataTemplate;
        Task Deregister<T>() where T : IDataTemplate;

        Task Register(Type type, IDataTemplateCollection collection);
        Task Deregister(Type type);

        T Get<T>(int id) where T : IDataTemplate;
        IEnumerable<T> GetAll<T>() where T : IDataTemplate;

        Task<T> GetAsync<T>(int id) where T : IDataTemplate;
        Task<IEnumerable<T>> GetAllAsync<T>() where T : IDataTemplate;
    }
}