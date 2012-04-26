using System;
using NUnit.Framework;
using Besnik.GenericRepository.NHibernate;
using Besnik.Domain.NHibernateRepository;
using System.IO;

namespace Besnik.GenericRepository.Tests
{
	/// <summary>
	/// Base class for nhibernate unit of work fixtures.
	/// </summary>
	public abstract class NHibernateUnitOfWorkFixture : BaseUnitOfWorkFixture
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
			(this.UnitOfWorkFactory as NHibernateUnitOfWorkFactory).NHibernateSchemaExport();
		}

		protected virtual string NHibernateConfigPath
		{
			get
			{
				return Factory.NHibernateConfigPath;
			}
		}

		/// <summary>
		/// Gets absolute path to the NHibernate config path.
		/// </summary>
		protected string GetNHibernateConfigPath()
		{
			return Path.Combine(
				Environment.CurrentDirectory
				, NHibernateConfigPath
				);
		}
	}
}
