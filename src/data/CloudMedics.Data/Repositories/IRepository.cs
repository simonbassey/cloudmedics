using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudMedics.Data
{
    public interface IRepository<T> where T : class
    {
        Task<T> Add(T entity);
        Task<int> Add(IEnumerable<T> tEntities);
        Task<T> Get(object entityId);
        Task<List<T>> GetAll();

        Task<T> Update(T updatedEntity);
        Task<bool> Delete(object key);
        Task<IEnumerable<T>> Filter(Func<T, bool> predicate);
    }
}
