using System;
using System.Reflection;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace Besnik.GenericRepository.NHibernate
{
	public class NHibernateUnitOfWorkFactory : IUnitOfWorkFactory
	{
		public NHibernateUnitOfWorkFactory(string nhibernateConfigPath, Assembly assembly)
		{
			var cfg = new Configuration();

			try
			{
				// configuration is immutable, store last returned value
				cfg.Configure(nhibernateConfigPath);
				Configuration = cfg.AddAssembly(assembly);
			}
			catch ( Exception ex )
			{
				throw new GenericRepositoryException(
					string.Format("Error while configuring NHibernate: {0}.", ex.Message)
					, ex
					);
			}

			try
			{
				SessionFactory = Configuration.BuildSessionFactory();
			}
			catch ( Exception ex )
			{
				throw new GenericRepositoryException(
					string.Format("Error while building NH session factory: {0}.", ex.Message)
					, ex
					);
			}
		}

		protected ISessionFactory SessionFactory { get; private set; }

		protected Configuration Configuration { get; private set; }

		/// <summary>
		/// Generates table structure inside specified database.
		/// </summary>
		public void NHibernateSchemaExport()
		{
			new SchemaExport(this.Configuration).Execute(false, true, false);
		}

		#region IUnitOfWorkFactory Members

		public IUnitOfWork BeginUnitOfWork()
		{
			return new NHibernateUnitOfWork( this.SessionFactory.OpenSession() );
		}

		public void EndUnitOfWork(IUnitOfWork unitOfWork)
		{
			var nhUnitOfWork = unitOfWork as NHibernateUnitOfWork;
			if ( unitOfWork != null )
			{
				unitOfWork.Dispose();
				unitOfWork = null;
			}
		}

		#endregion

		#region IDisposable Members

		public void Dispose()
		{
			if ( this.SessionFactory != null )
			{
				(this.SessionFactory as IDisposable).Dispose();
				this.SessionFactory = null;
				this.Configuration = null;
			}
		}

		#endregion
	}
}
