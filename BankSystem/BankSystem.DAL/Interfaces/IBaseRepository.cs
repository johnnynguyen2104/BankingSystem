using BankSystem.DAL.DomainModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;


namespace BankSystem.DAL.Interfaces
{
    public interface IBaseRepository<TKey, TEntity> where TEntity : BaseEntity<TKey> where TKey : struct
    {

        /// <summary>
        /// Returning list of entities base on expression
        /// </summary>
        /// <param name="expression">Condition for returning list of entities</param>
        /// <returns>Returning list of entities</returns>
        IQueryable<TEntity> Read(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Returning an entity base on expression
        /// </summary>
        /// <param name="expression">Condition for returning an entity</param>
        /// <returns>Returning an entity</returns>
        TEntity ReadOne(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Creating an entity.
        /// </summary>
        /// <param name="entity">Entity need to create</param>
        /// <returns>Returning a created entity </returns>
        TEntity Create(TEntity entity);

        /// <summary>
        /// Updating an entity
        /// </summary>
        /// <param name="entity">Entity for updating</param>
        int Update(TEntity entity);

        /// <summary>
        /// Deleting list of entities base on condition
        /// </summary>
        /// <param name="expression">Condition for deleting list of entities</param>
        /// <returns>Returning number as result (1 as success, below 1 as fail)</returns>
        int Delete(Expression<Func<TEntity, bool>> expression);

        /// <summary>
        /// Committing all the changes we have made to database
        /// </summary>
        /// <returns>Returning number as result (1 as success, below 1 as fail)</returns>
        int CommitChanges();
    }
}
