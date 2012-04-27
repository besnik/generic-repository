using System;
using NUnit.Framework;
using Besnik.GenericRepository.EntityFramework;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using Besnik.Domain.EntityFramework;
using Besnik.Domain;
using System.Data.Entity;
using System.Data;

namespace Besnik.GenericRepository.Tests
{
	[TestFixture]
	public class EntityFrameworkCustomerRepositoryFixture : GenericCustomerRepositoryFixture
	{
		protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
		{
			var dbInitializer = new DropCreateDatabaseIfModelChanges<DbContext>();
			Database.SetInitializer(dbInitializer);

			return new EntityFrameworkUnitOfWorkFactory(
				this.GetConnectionString()
				, this.GetDbModel()
				);
		}

		protected override void InitializeDataStorage()
		{
			using (var uow = this.UnitOfWorkFactory.BeginUnitOfWork())
			{
				var dbContext = (uow as EntityFrameworkUnitOfWork).DbContext;
				dbContext.Database.Initialize(false);

				// clear db manually as EF 4 Extensions CTP4 does not contain any functionality
				// except deleting and creating database which is not what we want.
				var connection = dbContext.Database.Connection;

				try
				{
					connection.Open();

					using (var dbCommand = connection.CreateCommand())
					{
						dbCommand.CommandText = "delete from Customers";
						dbCommand.ExecuteNonQuery();
					}
				}
				finally
				{
					connection.Close();
				}
			}
		}

		protected override ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork)
		{
			var specificationLocator = this.Factory.GetSpecificationLocatorForEntityFramework();
			return new CustomerRepository(unitOfWork, specificationLocator);
		}

		protected override Customer GetCustomer(int customerId, IUnitOfWork unitOfWork)
		{
			var dbContext = (unitOfWork as EntityFrameworkUnitOfWork).DbContext;
			var customerSet = dbContext.Set<Customer>();
			return customerSet.Find(customerId);
		}

		/// <summary>
		/// Gets key in connection strings section used to load connection string from app.config.
		/// </summary>
		protected virtual string ConnectionStringKey
		{
			get
			{
				return "EntityFrameworkConnectionString";
			}
		}

		/// <summary>
		/// Gets connection string from config file used to initialize unit of work factory.
		/// </summary>
		protected virtual string GetConnectionString()
		{
			return this.Configuration.ConnectionStrings.ConnectionStrings[ConnectionStringKey].ConnectionString;
		}

		/// <summary>
		/// Gets entity framework's db model for the domain model.
		/// </summary>
		/// <returns></returns>
		protected virtual DbModel GetDbModel()
		{
			var modelBuilder = new DbModelBuilder();

			modelBuilder.Configurations.Add(new CustomerConfiguration());

			return modelBuilder.Build(new DbProviderInfo("System.Data.SqlClient", "2008"));
		}
	}
}
