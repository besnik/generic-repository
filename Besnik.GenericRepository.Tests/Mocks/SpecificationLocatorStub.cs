using Autofac;
using Besnik.GenericRepository;

namespace Besnik.GenericRepository.Tests
{
	public class SpecificationLocatorStub : ISpecificationLocator
	{
		public SpecificationLocatorStub(IContainer container)
		{
			this.Container = container;
		}

		protected IContainer Container { get; private set; }

		public TSpecification Resolve<TSpecification, TEntity>()
			where TSpecification : ISpecification<TEntity>
			where TEntity : class
		{
			return this.Container.Resolve<TSpecification>();
		}
	}
}
