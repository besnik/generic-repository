using Besnik.GenericRepository.NHibernate;
using NHibernate.Criterion;

namespace Besnik.Domain.NHibernateRepository
{
	/// <summary>
	/// Customer specification based on NHibernate criteria queries functionality.
	/// </summary>
	public class CriteriaCustomerSpecification : CriteriaSpecification<Customer>, ICustomerSpecification
	{
		public ICustomerSpecification WithName(string name)
		{
			this.Criteria.Add(Expression.Eq("Name", name));
			return this;
		}

		public ICustomerSpecification WithAge(int age)
		{
			this.Criteria.Add(Expression.Eq("Age", age));
			return this;
		}
	}
}
