using System;
using System.Collections.Generic;
using NHibernate;

namespace Besnik.GenericRepository.NHibernate
{
	public class NHibernateUnitOfWork : IUnitOfWork
	{
		public NHibernateUnitOfWork(ISession session)
		{
			this.Session = session;
		}

		public ISession Session { get; private set; }

		#region IUnitOfWork Members

		public void Flush()
		{
			this.Session.Flush();
		}

		public GenericRepository.ITransaction BeginTransaction()
		{
			return new NHibernateTransaction(
				this.Session.BeginTransaction()
				);
		}

		public void EndTransaction(GenericRepository.ITransaction transaction)
		{
			if (transaction != null)
			{
				(transaction as IDisposable).Dispose();
				transaction = null;
			}
		}

		public void Insert<TEntity>(TEntity entity) where TEntity : class
		{
			this.Session.Save(entity);
		}

		public void Update<TEntity>(TEntity entity) where TEntity : class
		{
			this.Session.Update(entity);
		}

		public void Delete<TEntity>(TEntity entity) where TEntity : class
		{
			this.Session.Delete(entity);
		}

		public TEntity GetById<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : class
		{
			return this.Session.Get<TEntity>(id);
		}

		public IList<TEntity> GetAll<TEntity>() where TEntity : class
		{
			return this.Session.CreateCriteria<TEntity>().List<TEntity>();
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if ( this.Session != null )
			{
				(this.Session as IDisposable).Dispose();
				this.Session = null;
			}
		}

		#endregion

	}
}
