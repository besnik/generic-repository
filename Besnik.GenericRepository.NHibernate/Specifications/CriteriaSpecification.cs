using NHibernate;

namespace Besnik.GenericRepository.NHibernate
{
	public abstract class CriteriaSpecification<TEntity> : ISpecification<TEntity> where TEntity : class
	{
		/// <summary>
		/// Gets criteria specification.
		/// </summary>
		protected ICriteria Criteria { get; private set; }

		/// <summary>
		/// Initializes criteria for the specification.
		/// </summary>
		public void Initialize(IUnitOfWork unitOfWork)
		{
			this.Criteria = (unitOfWork as NHibernateUnitOfWork).Session.CreateCriteria<TEntity>();
		}

		/// <summary>
		/// Gets <see cref="CriteriaSpecificationResult"/>.
		/// </summary>
		/// <returns></returns>
		public ISpecificationResult<TEntity> ToResult()
		{
			return new CriteriaSpecificationResult<TEntity>(this.Criteria);
		}
	}
}
