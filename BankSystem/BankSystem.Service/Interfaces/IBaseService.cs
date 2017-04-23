using BankSystem.Service.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace BankSystem.Service.Interfaces
{
    public interface IBaseService<TKey, TEntity> 
        where TEntity : BaseDto<TKey> where TKey : struct
    {

        /// <summary>
        /// Returning an entity base on expression
        /// </summary>
        /// <param name="id">id belong to an entity</param>
        /// <returns>Returning an entity</returns>
        TEntity ReadOneById(TKey id);

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
        void Update(TEntity entity);

        /// <summary>
        /// Deleting list of entities base on condition
        /// </summary>
        /// <param name="expression">Condition for deleting list of entities</param>
        /// <returns>Returning number as result (1 as success, below 1 as fail)</returns>
        int Delete(IList<TKey> ids);
    }
}
