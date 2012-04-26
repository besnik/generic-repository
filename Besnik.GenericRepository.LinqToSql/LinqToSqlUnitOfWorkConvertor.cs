using System.Linq;

namespace Besnik.GenericRepository.LinqToSql
{
	/// <summary>
	/// Helper class to convert LinqToSql DataContext to various data providers.
	/// </summary>
	public class LinqToSqlUnitOfWorkConvertor : IUnitOfWorkConvertor
	{
		/// <summary>
		/// Gets <see cref="IQueryable"/> from <see cref="DataContext"/> wrapped
		/// in the given <see cref="LinqToSqlUnitOfWork"/> instance.
		/// </summary>
		public IQueryable<TEntity> ToQueryable<TEntity>(IUnitOfWork unitOfWork) where TEntity : class
		{
			return (unitOfWork as LinqToSqlUnitOfWork).DataContext.GetTable<TEntity>();
		}
	}
}
