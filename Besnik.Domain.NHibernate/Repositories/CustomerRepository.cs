using Besnik.GenericRepository;

namespace Besnik.Domain.NHibernateRepository
{
	/// <summary>
	/// Customer repository based on NHibernate.
	/// </summary>
	public class CustomerRepository : GenericRepository<Customer, int>, ICustomerRepository
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public CustomerRepository(IUnitOfWork unitOfWork, ISpecificationLocator specificationLocator)
			: base(unitOfWork, specificationLocator)
		{
		}

	}
}
