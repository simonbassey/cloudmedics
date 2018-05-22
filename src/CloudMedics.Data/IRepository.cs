using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudMedics.Data
{
    public interface IRepository<T> where T: class
    {
        Task<T> Add(T entity);
        Task<int> Add(ICollection<T> entities);
        Task<int> Add(IEnumerable<T> tEntities);
        Task<T> Get(int entityId);
        Task<T> Get(string entityId);
        Task<IEnumerable<T>> GetAll();

        Task<T> Update(T updatedEntity);
        Task<T> Delete(int id);
        Task<IEnumerable<T>> Filter(Func<T, bool> predicate);
    }
}
