using System;
using System.Configuration;
using System.IO;
using System.Reflection;
using NUnit.Framework;
using Besnik.Domain;
using Besnik.GenericRepository;
using System.Collections.Generic;

namespace Besnik.GenericRepository.Tests
{
	/// <summary>
	/// Base class for customer repository integration tests.
	/// Derive and implement abstract functionality with concrete unit of work implementation.
	/// Do not forget decorate derived class with [TestFixture] attribute in order to execute
	/// the below generic tests.
	/// </summary>
	public abstract class GenericCustomerRepositoryFixture : BaseUnitOfWorkFixture
	{
		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_ExplicitFlush()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);

				unitOfWork.Flush();
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_NoFlush()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void AddItemToRepository_NoFlushWithException()
		{
			// arrange
			var customer = this.Factory.GetCustomer();
			Assert.That(customer.Id, Is.EqualTo(0));

			// act
			try
			{
				using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
				{
					ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
					cr.Insert(customer);

					throw new Exception("mocked ex");
				}
			}
			catch
			{
				// assert
				using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
				{
					var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

					Assert.That(customerFromDb, Is.Not.Null);
					Assert.That(customerFromDb, Is.Not.SameAs(customer));
					Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
					Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
				}
			}

			
		}

		/// <summary>
		/// Template test method for adding customer into repository in a transaction.
		/// </summary>
		[Test]
		public void AddItemsToRepositoryInTransaction()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);

				using ( var transaction = unitOfWork.BeginTransaction() )
				{
					cr.Insert(customer);

					transaction.Commit();
				}
			}

			// assert
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				var customerFromDb = this.GetCustomer(customer.Id, unitOfWork);

				Assert.That(customerFromDb, Is.Not.Null);
				Assert.That(customerFromDb, Is.Not.SameAs(customer));
				Assert.That(customerFromDb.Name, Is.EqualTo(customer.Name));
				Assert.That(customerFromDb.Age, Is.EqualTo(customer.Age));
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository in a transaction that rolls back
		/// after an exception.
		/// </summary>
		[Test]
		public void AddItemsToRepositoryInTransaction_Rollback()
		{
			// arrange
			var customer = this.Factory.GetCustomer();

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				try
				{
					ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);

					using ( var transaction = unitOfWork.BeginTransaction() )
					{
						cr.Insert(customer);
						throw new Exception("mocked ex");
					}
				}
				catch ( Exception ex )
				{
					// assert
					Assert.That(ex.Message, Is.EqualTo("mocked ex"));

					using ( var innerUnitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
					{
						var customerFromDb = this.GetCustomer(customer.Id, innerUnitOfWork);

						Assert.That(customerFromDb, Is.Null);
					}

					return;
				}
			}
		}

		/// <summary>
		/// Template test method for adding customer into repository.
		/// </summary>
		[Test]
		public void GetUsingSpecifiedAge()
		{
			// arrange
			var customer = this.Factory.GetCustomer();
			
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer);
			}

			Customer c = null;

			// act
			using ( var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork() )
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

		/// <summary>
		/// Template test method for tesing Single method. In case the sequence contains
		/// no entities, the <see cref="InvalidOperationException"/> should be raised.
		/// </summary>
		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SingleShouldThrow()
		{
			// arrange
			Customer c = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				c = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.Single();
			}

			// assert
			Assert.Fail();
		}

		/// <summary>
		/// Template test method for tesing SingleOrDefault method. In case the sequence contains
		/// no entities, null should be the result.
		/// </summary>
		[Test]
		public void SingleOrDefaultShouldNotThrow()
		{
			// arrange
			Customer c = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				c = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.SingleOrDefault();
			}

			// assert
			Assert.That(c, Is.Null);
		}

		/// <summary>
		/// Template test method for ascending ordering.
		/// </summary>
		[Test]
		public void GetCustomersOrderByAgeAscending()
		{
			// arrange
			var customer1 = this.Factory.GetCustomer("Peter Bondra", 38);
			var customer2 = this.Factory.GetCustomer("Miroslav Satan", 32);
			var customer3 = this.Factory.GetCustomer("Zigmund Palffy", 34);

			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer1);
				cr.Insert(customer2);
				cr.Insert(customer3);
			}

			IList<Customer> customers = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				customers = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.OrderByAscending(c => c.Age)
					.ToList();
			}

			// assert
			Assert.That(customers, Is.Not.Null);
			Assert.That(customers.Count, Is.EqualTo(3));
			Assert.That(customers[0].Age, Is.LessThanOrEqualTo(customers[1].Age));
			Assert.That(customers[1].Age, Is.LessThanOrEqualTo(customers[2].Age));
		}

		/// <summary>
		/// Template test method for descending ordering.
		/// </summary>
		[Test]
		public void GetCustomersOrderByAgeDescending()
		{
			// arrange
			var customer1 = this.Factory.GetCustomer("Peter Bondra", 38);
			var customer2 = this.Factory.GetCustomer("Miroslav Satan", 32);
			var customer3 = this.Factory.GetCustomer("Zigmund Palffy", 34);

			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer1);
				cr.Insert(customer2);
				cr.Insert(customer3);
			}

			IList<Customer> customers = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				customers = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.OrderByDescending(c => c.Age)
					.ToList();
			}

			// assert
			Assert.That(customers, Is.Not.Null);
			Assert.That(customers.Count, Is.EqualTo(3));
			Assert.That(customers[0].Age, Is.GreaterThanOrEqualTo(customers[1].Age));
			Assert.That(customers[1].Age, Is.GreaterThanOrEqualTo(customers[2].Age));
		}

		/// <summary>
		/// Template test method for checking Skip functionality.
		/// </summary>
		[Test]
		public void GetCustomersButSkipSome()
		{
			// arrange
			var customer1 = this.Factory.GetCustomer("Peter Bondra", 38);
			var customer2 = this.Factory.GetCustomer("Miroslav Satan", 32);
			var customer3 = this.Factory.GetCustomer("Zigmund Palffy", 34);

			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				cr.Insert(customer1);
				cr.Insert(customer2);
				cr.Insert(customer3);
			}

			IList<Customer> customers = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				/* Important: only NHibernate correctly process the result.
				 * 
				 * Entity framework: before Skip() the OrderBy has to be called!
				 * 
				 * Linq2Sql: if orderby is called after Skip(), the order by is ignored then!
				 * 
				 * Note: try to remove OrderByAscending, you will see that only NHibernate and
				 * Linq2Sql is able to process the request.
				 * 
				 * Note2: try to put OrderByAscending AFTER Skip() and adapt last assert. 
				 * Only NHibernate is able to return correct result. Linq2Sql ignores ordering!
				 */
				customers = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.OrderByAscending(c => c.Age)
					.Skip(2)
					.ToList();
			}

			// assert
			Assert.That(customers, Is.Not.Null);
			Assert.That(customers.Count, Is.EqualTo(1));
			Assert.That(customers[0].Age, Is.EqualTo(38));
		}

		/// <summary>
		/// Template test method for ToList method if collection of entities is empty.
		/// Specification is used to select entities.
		/// </summary>
		[Test]
		public void GetCustomersToListButReceivedNoData()
		{
			// arrange

			IList<Customer> customers = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				customers = cr.Specify<ICustomerSpecification>()
					.ToResult()
					.ToList();
			}

			// assert
			Assert.That(customers, Is.Not.Null);
			Assert.That(customers.Count, Is.EqualTo(0));
		}

		/// <summary>
		/// Template test method for GetAll method if collection of entities is empty.
		/// </summary>
		[Test]
		public void GetAllCustomersIfReceivedNoData()
		{
			// arrange

			IList<Customer> customers = null;

			// act
			using (var unitOfWork = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				ICustomerRepository cr = this.CreateCustomerRepository(unitOfWork);
				customers = cr.GetAll();
			}

			// assert
			Assert.That(customers, Is.Not.Null);
			Assert.That(customers.Count, Is.EqualTo(0));
		}

		/// <summary>
		/// Creates unit of work implementation specific repository.
		/// </summary>
		protected abstract ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork);

		/// <summary>
		/// Loads customer from given data storage.
		/// </summary>
		protected abstract Customer GetCustomer(int customerId, IUnitOfWork unitOfWork);
	}
}
