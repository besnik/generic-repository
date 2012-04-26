using Besnik.GenericRepository;

namespace Besnik.Domain
{
	/// <summary>
	/// Customer specification using fluent interface.
	/// </summary>
	public interface ICustomerSpecification : ISpecification<Customer>
	{
		ICustomerSpecification WithName(string name);
		ICustomerSpecification WithAge(int age);
	}
}
