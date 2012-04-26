using System;
using System.Collections.Generic;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// The interface represents unit of work pattern implementation.
	/// </summary>
	public interface IUnitOfWork : IDisposable
	{
		/// <summary>
		/// Flushes content of unit of work to the underlying data storage. Causes unsaved
		/// entities to be written to the data storage.
		/// </summary>
		void Flush();

		/// <summary>
		/// Begins the transaction.
		/// </summary>
		ITransaction BeginTransaction();

		/// <summary>
		/// Ends transaction.
		/// Note: suggested pattern to manage a transaction is via *using* construct.
		/// You should set input param to null after calling the method.
		/// </summary>
		/// <example>
		/// using ( var tnx = uow.BeginTransaction() ) { /* do some work */ }
		/// </example>
		/// See also <seealso cref="ITransaction"/> interface for more details.
		void EndTransaction(ITransaction transaction);

		/// <summary>
		/// Inserts entity to the storage.
		/// </summary>
		void Insert<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Updates entity in the storage.
		/// </summary>
		void Update<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Deletes entity in the storage.
		/// </summary>
		void Delete<TEntity>(TEntity entity) where TEntity : class;

		/// <summary>
		/// Gets entity from the storage by it's Id.
		/// </summary>
		TEntity GetById<TEntity, TPrimaryKey>(TPrimaryKey id) where TEntity : class;

		/// <summary>
		/// Gets all entities of the type from the storage. 
		/// </summary>
		IList<TEntity> GetAll<TEntity>() where TEntity : class;
	}
}
