using System.Linq;
using NHibernate.Linq;

namespace Besnik.GenericRepository.NHibernate
{
	/// <summary>
	/// Helper class to convert NHibernate ISession to various data providers.
	/// </summary>
	public class NHibernateUnitOfWorkConvertor : IUnitOfWorkConvertor
	{
		/// <summary>
		/// Gets <see cref="INHibernateQueryable"/> from <see cref="ISession"/> wrapped
		/// in the given <see cref="NHibernateUnitOfWork"/> instance.
		/// </summary>
		public IQueryable<TEntity> ToQueryable<TEntity>(IUnitOfWork unitOfWork) where TEntity : class
		{
			return (unitOfWork as NHibernateUnitOfWork).Session.Query<TEntity>();
		}
	}
}
