using System.Data.Linq;
using System.Data.Linq.Mapping;

namespace Besnik.GenericRepository.LinqToSql
{
	public class LinqToSqlUnitOfWorkFactory : IUnitOfWorkFactory
	{
		public LinqToSqlUnitOfWorkFactory(string connectionString, MappingSource mapping)
		{
			this.ConnectionString = connectionString;
			this.Mapping = mapping;
		}

		protected string ConnectionString { get; private set; }

		protected MappingSource Mapping { get; private set; }

		protected DataContext CreateDataContext()
		{
			return new DataContext(
				this.ConnectionString
				, this.Mapping
				);
		}

		public IUnitOfWork BeginUnitOfWork()
		{
			return new LinqToSqlUnitOfWork(
				this.CreateDataContext()
				);
		}

		public void EndUnitOfWork(IUnitOfWork unitOfWork)
		{
			var linqToSqlUnitOfWork = unitOfWork as LinqToSqlUnitOfWork;
			if ( linqToSqlUnitOfWork != null )
			{
				linqToSqlUnitOfWork.Dispose();
				linqToSqlUnitOfWork = null;
			}
		}

		public void Dispose()
		{
			this.ConnectionString = null;
			this.Mapping = null;
		}
	}
}
