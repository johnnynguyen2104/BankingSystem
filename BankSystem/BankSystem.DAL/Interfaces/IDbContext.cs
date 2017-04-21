using BankSystem.DAL.DomainModel;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankSystem.DAL.Interfaces
{
    public interface IDbContext
    {
        DbSet<TEntity> Set<TKey, TEntity>() where TEntity : BaseEntity<TKey> where TKey : struct;

        EntityEntry<TEntity> Entry<TKey, TEntity>(TEntity entity) where TEntity : BaseEntity<TKey> where TKey : struct;

        int CommitChanges();
    }
}
