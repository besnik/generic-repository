using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// The interface for wrapps concrete implementation of unit of work pattern.
	/// </summary>
	public interface IUnitOfWorkFactory : IDisposable
	{
		/// <summary>
		/// Begins unit of work.
		/// </summary>
		IUnitOfWork BeginUnitOfWork();

		/// <summary>
		/// Ends unit of work.
		/// </summary>
		void EndUnitOfWork(IUnitOfWork unitOfWork);
	}
}
