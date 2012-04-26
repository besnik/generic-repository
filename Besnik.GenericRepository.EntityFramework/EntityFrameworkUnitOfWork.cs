using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.Entity;

namespace Besnik.GenericRepository.EntityFramework
{
    public class EntityFrameworkUnitOfWork : IUnitOfWork
    {
        public EntityFrameworkUnitOfWork(DbContext dbContext)
        {
            this.DbContext = dbContext;
        }

        public DbContext DbContext { get; protected set; }

        public void Flush()
        {
            this.DbContext.SaveChanges();
        }

        public ITransaction BeginTransaction()
        {
            return new EntityFrameworkTransaction(this);
        }

        public void EndTransaction(ITransaction transaction)
        {
            if (transaction != null)
            {
                (transaction as IDisposable).Dispose();
                transaction = null;
            }
        }

        public void Insert<TEntity>(TEntity entity) where TEntity : class
        {
            this.DbContext.Set<TEntity>().Add(entity);
        }

        public void Update<TEntity>(TEntity entity) where TEntity : class
        {
            this.DbContext.Set<TEntity>().Attach(entity);
        }

        public void Delete<TEntity>(TEntity entity) where TEntity : class
        {
            this.DbContext.Set<TEntity>().Remove(entity);
        }

        public TEntity GetById<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : class
        {
            return this.DbContext.Set<TEntity>().Find(id);
        }

        public IList<TEntity> GetAll<TEntity>() where TEntity : class
        {
            return this.DbContext.Set<TEntity>().ToList();
        }

        public void Dispose()
        {
            if (this.DbContext != null)
            {
                this.DbContext.SaveChanges();
                (this.DbContext as IDisposable).Dispose();
                this.DbContext = null;
            }
        }
    }
}
