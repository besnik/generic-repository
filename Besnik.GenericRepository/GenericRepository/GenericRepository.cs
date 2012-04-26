using System;
using System.Collections.Generic;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic repository wraps given <see cref="IUnitOfWork"/> implementation
	/// and provides unified access to the entities stored in underlying data storage.
	/// </summary>
	/// <remarks>
	/// Additionally to <see cref="IUnitOfWork"/>, the repository supports 
	/// fluently initialized specifications. See also <see cref="Specify"/> method.
	/// 
	/// All commands have to be executed over started unit of work session.
	///
	/// Flushing of entities to underlying data storage is in competence of 
	/// given unit of work. In other words, synchronization between in-memory repository
	/// and data storage (e.g. database) is done via unit of work. This way the client
	/// has complete control over calling data storage and can optimize the way the entities
	/// are managed.
	/// </remarks>
	public class GenericRepository<TEntity, TPrimaryKey> : IGenericRepository<TEntity, TPrimaryKey>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <param name="unitOfWork">Unit of work for concrete implementation of data mapper.</param>
		/// <param name="specificationLocator">Specification locator resolves implementations of
		/// <see cref="ISpecification"/> interface. <see cref="ISpecificationLocator"/> is normally
		/// wrapper over IoC container.</param>
		public GenericRepository(
			IUnitOfWork unitOfWork
			, ISpecificationLocator specificationLocator
			)
		{
			this.EnsureNotNull(specificationLocator);
			this.EnsureNotNull(unitOfWork);

			this.SpecificationLocator = specificationLocator;
			this.UnitOfWork = unitOfWork;
		}

		/// <summary>
		/// Checks if given instance is not null. Use the method to validate input parameters.
		/// </summary>
		protected void EnsureNotNull(object o)
		{
			if (o == null)
			{
				throw new ArgumentNullException("o", "Argument can not be null.");
			}
		}

		/// <summary>
		/// Gets specification locator for the repository to resolve specifications.
		/// </summary>
		protected ISpecificationLocator SpecificationLocator { get; private set; }

		/// <summary>
		/// Gets unit of work the repository operates on.
		/// </summary>
		protected IUnitOfWork UnitOfWork { get; private set; }

		/// <summary>
		/// Inserts entity to the repository.
		/// </summary>
		public virtual void Insert(TEntity entity)
		{
			this.UnitOfWork.Insert<TEntity>(entity);
		}

		/// <summary>
		/// Updates entity in the repository.
		/// </summary>
		public virtual void Update(TEntity entity)
		{
			this.UnitOfWork.Update<TEntity>(entity);
		}

		/// <summary>
		/// Deletes entity from the repository.
		/// </summary>
		public virtual void Delete(TEntity entity)
		{
			this.UnitOfWork.Delete<TEntity>(entity);
		}

		/// <summary>
		/// Gets entity from the repository by given id.
		/// </summary>
		/// <param name="id">Primary key that identifies the entity.</param>
		public virtual TEntity GetById(TPrimaryKey id)
		{
			return this.UnitOfWork.GetById<TEntity, TPrimaryKey>(id);
		}

		/// <summary>
		/// Gets all entities from the repository.
		/// </summary>
		public virtual IList<TEntity> GetAll()
		{
			return this.UnitOfWork.GetAll<TEntity>();
		}

		/// <summary>
		/// Gets specification that allows to filter only requested entities
		/// from the repository.
		/// </summary>
		/// <typeparam name="TSpecification">Concrete specification that will be resolved
		/// and initialized with underlying unit of work instance. This ensures fluent 
		/// and strongly typed way of connecting repository (uow) and specifications.</typeparam>
		public virtual TSpecification Specify<TSpecification>()
			where TSpecification : class, ISpecification<TEntity>
		{
			TSpecification specification = default(TSpecification);
			
			try
			{
				specification = this.SpecificationLocator.Resolve<TSpecification, TEntity>();
			}
			catch (Exception ex)
			{
				throw new GenericRepositoryException(
					string.Format(
						"Could not resolve requested specification {0} for entity {1} from the specification locator."
						, typeof(TSpecification).FullName 
						, typeof(TEntity).FullName
						)
					, ex
					);
			}

			specification.Initialize(UnitOfWork);
			return specification;
		}
	}
}
