using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BankSystem.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankSystem.DAL.DomainModel
{
    public class BankSystemDbContext : IdentityDbContext<User, Role, string>, IDbContext
    {
        public BankSystemDbContext(DbContextOptions<BankSystemDbContext> options) 
            : base(options)
        {

        }

        public BankSystemDbContext() : base()
        {

        }

        public int CommitChanges()
        {
            return this.SaveChanges();
        }

        public new EntityEntry<TEntity> Entry<TKey, TEntity>(TEntity entity)
            where TKey : struct
            where TEntity : BaseEntity<TKey>
        {
            return this.Entry<TEntity>(entity);
        }

        public new DbSet<TEntity> Set<TKey, TEntity>()
            where TKey : struct
            where TEntity : BaseEntity<TKey>
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>()
                .HasMany(a => a.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);
        }
    }
}
