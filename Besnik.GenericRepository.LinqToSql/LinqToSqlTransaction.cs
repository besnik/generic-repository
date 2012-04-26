using System;
using System.Transactions;

namespace Besnik.GenericRepository.LinqToSql
{
	/// <summary>
	/// LINQ to SQL implementation of transaction.
	/// </summary>
	public class LinqToSqlTransaction : ITransaction
	{
		public LinqToSqlTransaction(LinqToSqlUnitOfWork unitOfWork)
		{
			this.UnitOfWork = unitOfWork;
			this.TransactionScope = new TransactionScope();
		}

		protected LinqToSqlUnitOfWork UnitOfWork { get; private set; }

		protected TransactionScope TransactionScope { get; private set; }

		#region ITransaction Members

		/// <summary>
		/// Flushes unit of work and commits the transaction scope.
		/// </summary>
		public void Commit()
		{
			this.UnitOfWork.Flush();
			this.TransactionScope.Complete();
		}

		/// <summary>
		/// Rolls back transaction.
		/// Actually the transaction rollback is handled automatically with Dispose method if
		/// transaction scope was not commited.
		/// </summary>
		public void Rollback()
		{
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if ( this.TransactionScope != null )
			{
				(this.TransactionScope as IDisposable).Dispose();
				this.TransactionScope = null;
				this.UnitOfWork = null;
			}
		}

		#endregion
	}
}
