using System;
using System.Linq.Expressions;
using Besnik.GenericRepository;
using Besnik.GenericRepository.LinqToSql;

namespace Besnik.Domain.LinqToSqlRepository
{
	/// <summary>
	/// Customer repository based on LinqToSql technology.
	/// </summary>
	public class CustomerRepository : LinqToSqlRepository<Customer, int>, ICustomerRepository
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public CustomerRepository(IUnitOfWork unitOfWork, ISpecificationLocator specificationLocator)
			: base(unitOfWork, specificationLocator)
		{
		}

		/// <summary>
		/// Gets predicate that filters entity using given id.
		/// </summary>
		protected override Expression<Func<Customer, bool>> FirstOrDefaultPredicate(int id)
		{
			return c => c.Id == id;
		}
	}
}
