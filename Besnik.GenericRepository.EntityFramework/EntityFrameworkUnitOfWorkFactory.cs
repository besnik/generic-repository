using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace Besnik.GenericRepository.EntityFramework
{
    /// <summary>
    /// Unit of work factory implementation for Entity Framework.
    /// Note: implementation based on extension Feature CTP4.
    /// </summary>
    public class EntityFrameworkUnitOfWorkFactory : IUnitOfWorkFactory
    {
        public EntityFrameworkUnitOfWorkFactory(string connectionString, DbModel dbModel)
        {
            this.ConnectionString = connectionString;
			this.DbModel = dbModel;
        }

        protected string ConnectionString { get; private set; }

		protected DbModel DbModel { get; private set; }

        public IUnitOfWork BeginUnitOfWork()
        {
			return new EntityFrameworkUnitOfWork(
				this.CreateDbContext()
				);
        }

		private DbContext CreateDbContext()
		{
			return new DbContext(
				this.ConnectionString
				, this.DbModel.Compile()
				);
		}

        public void EndUnitOfWork(IUnitOfWork unitOfWork)
        {
			var linqToSqlUnitOfWork = unitOfWork as EntityFrameworkUnitOfWork;
			if (linqToSqlUnitOfWork != null)
			{
				linqToSqlUnitOfWork.Dispose();
				linqToSqlUnitOfWork = null;
			}
        }

        public void Dispose()
        {
			this.ConnectionString = null;
			this.DbModel = null;
        }
	}
}
