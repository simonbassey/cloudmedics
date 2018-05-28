using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CloudMedics.Data
{
    public abstract class BaseRepository<T> : IRepository<T> where T:class
    {
        public BaseRepository()
        {
        }

        public virtual async Task<T> Add(T entity)
        {
            try{
                using(var dbContext = new CloudMedicDbContext()){
                    await dbContext.Set<T>().AddAsync(entity);
                    await dbContext.SaveChangesAsync();
                    return entity;
                }
            }
            catch(Exception exception){
                throw;
            }
        }

        public virtual async Task<int> Add(IEnumerable<T> tEntities)
        {
            try
            {
                using (var dbContext = new CloudMedicDbContext())
                {
                    await dbContext.Set<T>().AddRangeAsync(tEntities);
                    return await dbContext.SaveChangesAsync();
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public virtual async Task<bool> Delete(object key)
        {
            try{
                using (var dbContext = new CloudMedicDbContext())
                {
                    var existingEntity = await dbContext.Set<T>().FindAsync(key);
                    if (existingEntity == null)
                        return true;
                    dbContext.Set<T>().Remove(existingEntity);
                    return (await dbContext.SaveChangesAsync()) == 1;
                }
            }
            catch(Exception exception){
                throw;
            }
        }

        public virtual async Task<IEnumerable<T>> Filter(Func<T, bool> predicate)
        {
            try
            {
                using (var dbContext = new CloudMedicDbContext())
                {
                    return await Task.FromResult(dbContext.Set<T>().Where(predicate));
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public virtual async Task<T> Get(object entityId)
        {

            try
            {
                using (var dbContext = new CloudMedicDbContext())
                {
                    return await dbContext.Set<T>().FindAsync(entityId);
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public virtual async Task<List<T>> GetAll()
        {
            try
            {
                using (var dbContext = new CloudMedicDbContext())
                {
                    return await Task.FromResult(dbContext.Set<T>().ToList());
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }

        public virtual async Task<T> Update(T updatedEntity)
        {
            try
            {
                using (var dbContext = new CloudMedicDbContext())
                {
                    dbContext.Entry<T>(updatedEntity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
                    await dbContext.SaveChangesAsync();
                    return updatedEntity;
                }
            }
            catch (Exception exception)
            {
                throw;
            }
        }
    }
}
