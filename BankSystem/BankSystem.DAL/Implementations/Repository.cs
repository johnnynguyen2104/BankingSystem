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

        public Repository()
        {
            DbContext = new BankSystemDbContext();
        }

        public IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression)
        {
            var result = DbContext.Set<TKey, TEntity>().Where(expression);

            return result;
        }

        public TEntity Create(TEntity entity)
        {
            DbContext.Entry<TKey, TEntity>(entity).State = Microsoft.EntityFrameworkCore.EntityState.Added;
            return entity;
        }

        public void Update(TEntity entity)
        {
            var updateEntity = Read(a => a.Id.Equals(entity.Id)).FirstOrDefault();
            DbContext.Entry<TKey, TEntity>(updateEntity).CurrentValues.SetValues(entity);
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
