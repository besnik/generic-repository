using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Represents transaction in the repository.
	/// Don't forget to dispose the transaction when you are done with the transaction.
	/// </summary>
	/// <remarks>
	/// Suggested pattern when working with transaction interface is as following:
	/// using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
	/// {
	///     ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
	/// 
	///     using ( var transaction = unitOfWork.BeginTransaction() )
	///     {
	///         cr.Insert(customer);
	/// 
	///         transaction.Commit();
	///     }
	/// }
	/// 
	/// If you need to work with transaction without using block, don't forget to rollback
	/// transaciton is catch block (if possible) and dispose the transaction in finally block:
	/// ITransaction transaction = null;
	/// try
	/// {
	///		transaction = unitOfWork.BeginTransaction()
	///		cr.Insert(customer);
	///		transaction.Commit();
	/// }
	/// catch
	/// {
	///		transaction.Rollback();
	///		throw;
	/// }
	/// finally
	/// {
	///		if (transaction != null) { transaction.Dispose(); }
	/// }
	/// </remarks>
	public interface ITransaction : IDisposable
	{
		/// <summary>
		/// Commits the transaction.
		/// </summary>
		void Commit();

		/// <summary>
		/// Rollbacks the transaction.
		/// </summary>
		void Rollback();
	}
}
