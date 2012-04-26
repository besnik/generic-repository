using System;
using INHibernateTransaction = global::NHibernate.ITransaction;

namespace Besnik.GenericRepository.NHibernate
{
	/// <summary>
	/// NHibernate implementation of transaction.
	/// </summary>
	public class NHibernateTransaction : GenericRepository.ITransaction
	{
		public NHibernateTransaction(INHibernateTransaction transaction)
		{
			this.Transaction = transaction;
		}

		protected INHibernateTransaction Transaction { get; private set; }

		public void Commit()
		{
			this.Transaction.Commit();
		}

		public void Rollback()
		{
			this.Transaction.Rollback();
		}
		
		public void Dispose()
		{
			if ( this.Transaction != null )
			{
				(this.Transaction as IDisposable).Dispose();
				this.Transaction = null;
			}
		}
	}
}
