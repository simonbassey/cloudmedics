using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudMedics.Data
{
    public interface IRepository<T> where T: class
    {
        Task<T> Add(T entity);
        Task<int> Add(IEnumerable<T> tEntities);
        Task<T> Get(int entityId);
        Task<IEnumerable<T>> GetAll();
        Task<IEnumerable<T>> Filter(Func<T, bool> predicate);
    }
}
