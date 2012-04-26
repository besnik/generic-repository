using Besnik.GenericRepository;

namespace Besnik.Domain.EntityFramework
{
	/// <summary>
	/// Customer repository based on Entity Framework.
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

