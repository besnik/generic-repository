using System;
using NUnit.Framework;
using Besnik.Domain;
using Besnik.Domain.NHibernateRepository;

namespace Besnik.GenericRepository.Tests
{
	[TestFixture]
	public class NHibernateCriteriaFixture : NHibernateUnitOfWorkFixture
	{
		[Test]
		public void CriteriaSpecificationTest()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);
			}

			Customer c = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				c = cr.Specify<ICustomerSpecification>()
					.WithAge(Factory.DefaultCustomerAge)
					.ToResult()
					.Single();
			}

			// assert
			Assert.That(c, Is.Not.Null);
			Assert.That(c, Is.Not.SameAs(customer));
			Assert.That(c.Name, Is.EqualTo(customer.Name));
			Assert.That(c.Age, Is.EqualTo(customer.Age));
		}

		protected ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork)
		{
			// repository
			var specificationLocator = this.Factory.GetSpecificationLocatorForNHibernateWithCriteria();
			return new CustomerRepository(unitOfWork, specificationLocator);
		}
	}
}
