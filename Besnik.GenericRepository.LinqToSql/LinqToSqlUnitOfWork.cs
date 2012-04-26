using System;
using System.Collections.Generic;
using System.Data.Linq;
using System.Linq;

namespace Besnik.GenericRepository.LinqToSql
{
	public class LinqToSqlUnitOfWork : IUnitOfWork
	{
		public LinqToSqlUnitOfWork(DataContext dataContext)
		{
			this.DataContext = dataContext;
		}

		public DataContext DataContext { get; private set; }

		protected Table<TEntity> GetTable<TEntity>() where TEntity : class
		{
			return this.DataContext.GetTable<TEntity>();
		}

		#region IUnitOfWork Members

		public void Flush()
		{
			this.DataContext.SubmitChanges();
		}

		public ITransaction BeginTransaction()
		{
			return new LinqToSqlTransaction(this);
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
			this.GetTable<TEntity>().InsertOnSubmit(entity);
		}

		public void Update<TEntity>(TEntity entity) where TEntity : class
		{
			// Attach with "true" to say this is a modified entity
			// and it can be checked for optimistic concurrency because
			// it has a column that is marked with "RowVersion" attribute
			this.GetTable<TEntity>().Attach(entity, true);
		}

		public void Delete<TEntity>(TEntity entity) where TEntity : class
		{
			this.GetTable<TEntity>().DeleteOnSubmit(entity);
		}

		public TEntity GetById<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : class
		{
			// Unit of work can not support this since LinqToSql implementation is 
			// based on Linq FirstOrDefault functionality that expects predicate
			// and here the details of TEntity are unknown.
			// The functionality is implemented directly in the LinqToSqlRepository type.
			throw new NotImplementedException("Implement directly in repository.");
		}

		public IList<TEntity> GetAll<TEntity>() where TEntity : class
		{
			return this.GetTable<TEntity>().ToList();
			
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if ( this.DataContext != null )
			{
				this.DataContext.SubmitChanges();
				(this.DataContext as IDisposable).Dispose();
				this.DataContext = null;
			}
		}

		#endregion

	}
}
