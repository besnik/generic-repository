using System.Linq;

namespace Besnik.GenericRepository.EntityFramework
{
	/// <summary>
	/// Helper class to convert Entity Framework DbContext to various data providers.
	/// </summary>
	public class EntityFrameworkUnitOfWorkConvertor : IUnitOfWorkConvertor
	{
		/// <summary>
		/// Gets <see cref="IQueryable"/> from <see cref="DataContext"/> wrapped
		/// in the given <see cref="LinqToSqlUnitOfWork"/> instance.
		/// </summary>
		public IQueryable<TEntity> ToQueryable<TEntity>(IUnitOfWork unitOfWork) where TEntity : class
		{
			return (unitOfWork as EntityFrameworkUnitOfWork).DbContext.Set<TEntity>();
		}
	}
}
