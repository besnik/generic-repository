using System;
using System.Linq;
using Besnik.GenericRepository;

namespace Besnik.Domain
{
	/// <summary>
	/// Generic specification based on IQueryable interface.
	/// </summary>
	public class CustomerQueryableSpecification
		: QueryableSpecification<Customer>
		, ICustomerSpecification
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public CustomerQueryableSpecification(IUnitOfWorkConvertor unitOfWorkConvertor)
			: base(unitOfWorkConvertor)
		{
		}

		/// <summary>
		/// Specifies the name the query will filter entities for.
		/// </summary>
		public ICustomerSpecification WithName(string name)
		{
			this.Queryable = this.Queryable.Where(c => c.Name == name);
			return this;
		}

		/// <summary>
		/// Specifies the age the query will filter entities for.
		/// </summary>
		public ICustomerSpecification WithAge(int age)
		{
			this.Queryable = this.Queryable.Where(c => c.Age == age);
			return this;
		}
	}
}
