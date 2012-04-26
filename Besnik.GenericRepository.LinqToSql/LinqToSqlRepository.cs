using System;
using System.Data.Linq;
using System.Linq;
using System.Linq.Expressions;

namespace Besnik.GenericRepository.LinqToSql
{
	/// <summary>
	/// Base class for repositories based on LinqToSql technology.
	/// </summary>
	/// <remarks>
	/// As the LinqToSql does not support general mechanism to get entity by id,
	/// we must extend base <see cref="GenericRepository"/> to add this functionality.
	/// It is done via predicate that specifies id of the entity.
	/// </remarks>
	public abstract class LinqToSqlRepository<TEntity, TPrimaryKey> 
		: GenericRepository<TEntity, TPrimaryKey>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public LinqToSqlRepository(IUnitOfWork unitOfWork, ISpecificationLocator specificationLocator)
			: base(unitOfWork, specificationLocator)
		{
		}

		/// <summary>
		/// Gets table of entities for the unit of work.
		/// </summary>
		protected Table<TEntity> Table
		{
			get
			{
				return ( this.UnitOfWork as LinqToSqlUnitOfWork ).DataContext.GetTable<TEntity>();
			}
		}

		/// <summary>
		/// Specifies predicate for <see cref="GetById"/> method.
		/// </summary>
		protected abstract Expression<Func<TEntity, bool>> FirstOrDefaultPredicate(TPrimaryKey id);

		/// <summary>
		/// Gets entity by it's Id.
		/// </summary>
		/// <remarks>
		/// The functionality has to be explicitely implemented here since we need concrete
		/// predicate for TEntity to select entity.
		/// </remarks>
		public override TEntity GetById(TPrimaryKey id)
		{
			return this.Table.FirstOrDefault(FirstOrDefaultPredicate(id));
		}

	}
}
