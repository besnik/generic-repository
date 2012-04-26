using System;
using System.Data.Linq;
using System.Data.Linq.Mapping;
using System.Linq;
using System.Reflection;
using Besnik.Domain;
using Besnik.GenericRepository;
using NUnit.Framework;
using Besnik.GenericRepository.LinqToSql;
using Besnik.Domain.LinqToSqlRepository;

namespace Besnik.GenericRepository.Tests
{
	[TestFixture]
	public class LinqToSqlCustomerRepositoryFixture : GenericCustomerRepositoryFixture
	{
		protected override IUnitOfWorkFactory CreateUnitOfWorkFactory()
		{
			return new LinqToSqlUnitOfWorkFactory(
				this.GetConnectionString()
				, this.GetMapping()
				);
		}

		protected override void InitializeDataStorage()
		{
			// next line ensures the domain assembly is loaded into app domain. 
			// this is necessary for linq2sql in order to correctly load xml mapping file.
			// since we haven't used domain functionaly anywhere yet, next line causes .NET to
			// load domain assmebly into app domain.
			Type t = typeof(ICustomerSpecification);

			var mapping = this.GetMapping();

			var tables = mapping.GetModel(typeof(DataContext)).GetTables();

			// deletes content of all tables
			foreach ( var table in tables )
			{
				using ( var unitOfWork = ( this.UnitOfWorkFactory as LinqToSqlUnitOfWorkFactory ).BeginUnitOfWork() )
				{
					( unitOfWork as LinqToSqlUnitOfWork ).DataContext.ExecuteQuery<object>(
						string.Format("delete from {0}", table.TableName)
						);

				}

			}
		}

		protected override ICustomerRepository CreateCustomerRepository(IUnitOfWork unitOfWork)
		{
			var specificationLocator = this.Factory.GetSpecificationLocatorForLinqToSql();
			return new CustomerRepository(unitOfWork, specificationLocator);
		}

		protected override Customer GetCustomer(int customerId, IUnitOfWork unitOfWork)
		{
			var dc = ( unitOfWork as LinqToSqlUnitOfWork ).DataContext;
			var table = dc.GetTable<Customer>();
			return table.FirstOrDefault(c => c.Id == customerId);
		}

		/// <summary>
		/// Gets key in connection strings section used to load connection string from app.config.
		/// </summary>
		protected virtual string ConnectionStringKey
		{
			get
			{
				return "Linq2SqlConnectionString";
			}
		}

		/// <summary>
		/// Gets connection string from config file used to initialize Linq2Sql data context.
		/// </summary>
		protected virtual string GetConnectionString()
		{
			return this.Configuration.ConnectionStrings.ConnectionStrings[ConnectionStringKey].ConnectionString;
		}

		/// <summary>
		/// Gets mapping to initialize Linq2Sql data context.
		/// </summary>
		/// <remarks>
		/// Fixture expects the linq2sql implementation contains one and only embedded resource file
		/// with mapping configuration.
		/// </remarks>
		protected virtual MappingSource GetMapping()
		{
			var executingAssembly = Assembly.GetAssembly(typeof(CustomerRepository));

			var customerMappingFileName =
				( from r in executingAssembly.GetManifestResourceNames().AsEnumerable()
				  where r.EndsWith(".linq2sql.xml")
				  select r ).Single();

			var customerMappingFile = executingAssembly.GetManifestResourceStream(customerMappingFileName);
			return XmlMappingSource.FromStream(customerMappingFile);
		}
	}
}

