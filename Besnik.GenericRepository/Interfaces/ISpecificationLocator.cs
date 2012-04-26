using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// The interface resolves specifications for a repository.
	/// The concrete implementation this usually wrapper over an IoC container.
	/// </summary>
	public interface ISpecificationLocator
	{
		/// <summary>
		/// Gets requested specification for given entity.
		/// </summary>
		TSpecification Resolve<TSpecification, TEntity>()
			where TSpecification : ISpecification<TEntity>
			where TEntity : class;
	}
}
