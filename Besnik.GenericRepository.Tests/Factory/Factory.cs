using Autofac;
using Besnik.Domain;
using Besnik.GenericRepository;
using System;
using Besnik.GenericRepository.NHibernate;
using Besnik.GenericRepository.LinqToSql;
using Besnik.Domain.NHibernateRepository;
using Besnik.GenericRepository.EntityFramework;

namespace Besnik.GenericRepository.Tests
{
	public class Factory
	{
		public static readonly string DefaultCustomerName = "John Doe";
		public static readonly int DefaultCustomerAge = 16;
		public static readonly string NHibernateConfigPath = "Config\\NHibernate.config";

		/// <summary>
		/// Gets customer stub.
		/// </summary>
		public Customer GetCustomer()
		{
			return GetCustomer(DefaultCustomerName, DefaultCustomerAge);
		}

		/// <summary>
		/// Gets customer stub with given name and age.
		/// </summary>
		public Customer GetCustomer(string name, int age)
		{
			return new Customer()
			{
				Name = name,
				Age = age
			};
		}

		public IUnitOfWorkConvertor GetNHibernateUnitOfWorkConvertor()
		{
			return new NHibernateUnitOfWorkConvertor();
		}

		public IUnitOfWorkConvertor GetLinqToSqlUnitOfWorkConvertor()
		{
			return new LinqToSqlUnitOfWorkConvertor();
		}

		/// <summary>
		/// Generic method that gets specification locator stub prepared for queryable 
		/// ICustomerSpecification. Use input type param to specify specification factory for
		/// concrete unit of work implementation that converts unit of work to specification.
		/// </summary>
		protected ISpecificationLocator GetSpecificationLocator(Action<ContainerBuilder> action)
		{
			// ioc container
			var builder = new ContainerBuilder();

			builder.RegisterType<CustomerQueryableSpecification>()
				.As<ICustomerSpecification>()
				.InstancePerDependency();

			action(builder);

			var container = builder.Build();

			// specification locator
			var specificationLocator = new SpecificationLocatorStub(container);

			return specificationLocator;
		}

		public ISpecificationLocator GetSpecificationLocatorForLinqToSql()
		{
			return GetSpecificationLocator(
				b =>
				{
					b.RegisterType<LinqToSqlUnitOfWorkConvertor>()
						.As<IUnitOfWorkConvertor>()
						.SingleInstance();
				});
		}

		public ISpecificationLocator GetSpecificationLocatorForEntityFramework()
		{
			return GetSpecificationLocator(
				b =>
				{
					b.RegisterType<EntityFrameworkUnitOfWorkConvertor>()
						.As<IUnitOfWorkConvertor>()
						.SingleInstance();
				});
		}

		public ISpecificationLocator GetSpecificationLocatorForNHibernate()
		{
			return GetSpecificationLocator(
				b =>
				{
					b.RegisterType<NHibernateUnitOfWorkConvertor>()
						.As<IUnitOfWorkConvertor>()
						.SingleInstance();
				});
		}

		public ISpecificationLocator GetSpecificationLocatorForNHibernateWithCriteria()
		{
			return GetSpecificationLocator(
				b =>
				{
					b.RegisterType<CriteriaCustomerSpecification>()
						.As<ICustomerSpecification>()
						.InstancePerDependency();
					b.RegisterType<NHibernateUnitOfWorkConvertor>()
						.As<IUnitOfWorkConvertor>()
						.SingleInstance();
				});
		}
	}
}
