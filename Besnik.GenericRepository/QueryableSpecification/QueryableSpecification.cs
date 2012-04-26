using System;
using System.Linq;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Base class for <see cref="IQueryable"/> based specifications.
	/// </summary>
	public abstract class QueryableSpecification<TEntity> : ISpecification<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="unitOfWorkConvertor">Interface provides functionality to convert
		/// unit of work to <see cref="IQueryable"/>.</param>
		public QueryableSpecification(IUnitOfWorkConvertor unitOfWorkConvertor)
		{
			if (unitOfWorkConvertor == null)
			{
				throw new ArgumentNullException("unitOfWorkConvertor");
			}

			this.UnitOfWorkConvertor = unitOfWorkConvertor;
		}

		/// <summary>
		/// Gets or sets the queryable instance.
		/// </summary>
		protected IQueryable<TEntity> Queryable { get; set; }

		/// <summary>
		/// Gets unit of work convertor.
		/// </summary>
		protected IUnitOfWorkConvertor UnitOfWorkConvertor { get; private set; }

		/// <summary>
		/// Initializes specification from given unit of work.
		/// Implementation varies on <see cref="IUnitOfWork"/> implementation.
		/// </summary>
		public virtual void Initialize(IUnitOfWork unitOfWork)
		{
			this.Queryable = this.UnitOfWorkConvertor.ToQueryable<TEntity>(unitOfWork);
		}

		/// <summary>
		/// Returns queryable specification result.
		/// </summary>
		public ISpecificationResult<TEntity> ToResult()
		{
			return new QueryableSpecificationResult<TEntity>(Queryable);
		}
	}
}
