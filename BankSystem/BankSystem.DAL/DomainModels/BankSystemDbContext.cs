using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using BankSystem.DAL.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace BankSystem.DAL.DomainModels
{
    public class BankSystemDbContext : IdentityDbContext<User, Role, string>, IDbContext
    {
        public BankSystemDbContext(DbContextOptions<BankSystemDbContext> options) 
            : base(options)
        {
        }

        public int CommitChanges()
        {
            return this.SaveChanges();
        }

        public EntityEntry<TEntity> Entry<TKey, TEntity>(TEntity entity)
            where TKey : struct
            where TEntity : BaseEntity<TKey>
        {
            return this.Entry<TEntity>(entity);
        }

        public DbSet<TEntity> Set<TKey, TEntity>()
            where TKey : struct
            where TEntity : BaseEntity<TKey>
        {
            return this.Set<TEntity>();
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<User>().ToTable("Users");
            builder.Entity<Role>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens");

            builder.Entity<User>()
                .HasMany(a => a.Accounts)
                .WithOne(a => a.User)
                .HasForeignKey(a => a.UserId);

            //set concurrency column
            builder.Entity<Account>()
                .Property(p => p.RowVersion).IsConcurrencyToken();

            builder.Entity<Account>().Property(e => e.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            builder.Entity<TransactionHistory>().Property(e => e.CreatedDate)
                .HasDefaultValueSql("getutcdate()");

            builder.Entity<Account>()
                .HasMany(a => a.Histories)
                .WithOne(a => a.Account)
                .HasForeignKey(a => a.AccountId)
                .IsRequired()
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict);

            builder.Entity<Account>()
                .HasMany(a => a.TransferredHistories)
                .WithOne(a => a.InteractionAccount)
                .HasForeignKey(a => a.InteractionAccountId)
                .OnDelete(Microsoft.EntityFrameworkCore.Metadata.DeleteBehavior.Restrict); ;
        }
    }
}
