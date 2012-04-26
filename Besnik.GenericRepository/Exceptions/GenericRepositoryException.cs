using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Signalizes general error in generic repository.
	/// </summary>
	public class GenericRepositoryException : Exception
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRepositoryException"/> class.
		/// </summary>
		public GenericRepositoryException()
			: base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRepositoryException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		public GenericRepositoryException(string message)
			: base(message)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="GenericRepositoryException"/> class.
		/// </summary>
		/// <param name="message">The error message.</param>
		/// <param name="innerException">The inner exception</param>
		public GenericRepositoryException(string message, Exception innerException)
			: base(message, innerException)
		{
		}
	}
}
