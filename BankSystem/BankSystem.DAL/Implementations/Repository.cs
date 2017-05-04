using BankSystem.DAL.DomainModels;
using BankSystem.DAL.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace BankSystem.DAL.Implementations
{
    public class Repository<TKey, TEntity> : IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey> where TKey : struct
    {
        private IDbContext DbContext { get; set; }

        public Repository(IDbContext dbContext)
        {
            DbContext = dbContext;
        }

        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression)
        {
            var result = DbContext.Set<TKey, TEntity>().Where(expression);

            return result;
        }

        public TEntity ReadOne(Expression<Func<TEntity, bool>> expression)
        {
            var entity = DbContext.Set<TKey, TEntity>().SingleOrDefault(expression);

            return entity;
        }

        public TEntity Create(TEntity entity)
        {
            DbContext.Entry<TKey, TEntity>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            return entity;
        }

        public int Update(TEntity entity)
        {
            var isExisted = Read(a => a.Id.Equals(entity.Id)).Count();
            if (isExisted > 0)
            {
                DbContext.Entry<TKey, TEntity>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Modified;

                return 1;
            }
            throw new KeyNotFoundException("Entity not found.");
        }

        public int Delete(Expression<Func<TEntity, bool>> expression)
        {
            var entities = DbContext.Set<TKey, TEntity>().Where(expression);

            foreach (var entity in entities)
            {
                DbContext.Set<TKey, TEntity>().Remove(entity);
            }

            return DbContext.CommitChanges();
        }

        public int CommitChanges()
        {
            return DbContext.CommitChanges();
        }
    }
}
