using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CloudMedics.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T:class
    {
        public BaseRepository()
        {
        }

        public virtual Task<T> Add(T entity)
        {
            throw new NotImplementedException();
        }

        public virtual Task<int> Add(IEnumerable<T> tEntities)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> Filter(Func<T, bool> predicate)
        {
            throw new NotImplementedException();
        }

        public virtual Task<T> Get(int entityId)
        {
            throw new NotImplementedException();
        }

        public virtual Task<IEnumerable<T>> GetAll()
        {
            throw new NotImplementedException();
        }
    }
}
