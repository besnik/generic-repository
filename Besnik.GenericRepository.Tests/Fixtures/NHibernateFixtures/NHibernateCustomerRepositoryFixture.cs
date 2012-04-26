using System;
using System.IO;
using NUnit.Framework;
using Besnik.Domain;
using Besnik.GenericRepository;
using Besnik.GenericRepository.NHibernate;
using Besnik.Domain.NHibernateRepository;

namespace Besnik.GenericRepository.Tests
{
	[TestFixture]
	public class NHibernateCustomerRepositoryFixture : GenericCustomerRepositoryFixture
	{
		protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
		{
			return new NHibernateUnitOfWorkFactory(
				this.GetNHibernateConfigPath()
				, typeof(CustomerRepository).Assembly
				);
		}

		protected override void InitializeDataStorage()
		{
			( this.UnitOfWorkFactory as NHibernateUnitOfWorkFactory ).NHibernateSchemaExport();
		}

		/// <summary>
		/// Gets absolute path to the NHibernate config path.
		/// </summary>
		protected virtual string GetNHibernateConfigPath()
		{
			return Path.Combine(
				Environment.CurrentDirectory
				, Factory.NHibernateConfigPath
				);
		}

		protected override ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork)
		{
			// repository
			var specificationLocator = this.Factory.GetSpecificationLocatorForNHibernate();
			return new CustomerRepository(unitOfWork, specificationLocator);
		}

		protected override Customer GetCustomer(int customerId, IUnitOfWork unitOfWork)
		{
			var session = ( unitOfWork as NHibernateUnitOfWork ).Session;
			return session.Get<Customer>(customerId);
		}
	}
}
