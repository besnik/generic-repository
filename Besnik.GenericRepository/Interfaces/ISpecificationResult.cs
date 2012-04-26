using System.Collections.Generic;
using System.Linq.Expressions;
using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Generic domain specification result interface that 
	/// contains methods common for filtering the result.
	/// </summary>
	/// <remarks>
	/// If the specification model uses lazy loading using <see cref="IQueryable<T>"/>, 
	/// this is the place where the query is translated and executed by calling for example
	/// <see cref="ToList"/> or <see cref="Single"/> methods.
	/// 
	/// The interface is very similar to IQueryable interface. It is basically
	/// the wrapper over the IQueryable if used under the hood. The main goal is to
	/// provide set of common methods used by all specifications. Other methods that
	/// normally IQueryable supports should be implemented in <see cref="ISpecification"/>
	/// interface using strong domain names of the method (e.g. joins, grouping, agregate functions).
	/// 
	/// Note it is perfectly fine if from domain perspective the specification already
	/// uses the functionality of this specification result. The specification should represents
	/// one query that can be customized using fluent interface but in general it should be
	/// strongly domain focused.
	/// </remarks>
	public interface ISpecificationResult<TEntity> where TEntity : class
	{
		/// <summary>
		/// Takes given count of domain objects.
		/// </summary>
		ISpecificationResult<TEntity> Take(int count);

		/// <summary>
		/// Bypasses a specified number of elements in a sequence and then returns 
		/// the remaining elements.
		/// </summary>
		/// <param name="count">The number of elements to skip before returning 
		/// the remaining elements.</param>
		ISpecificationResult<TEntity> Skip(int count);

		/// <summary>
		/// Sorts the elements of a sequence in ascending order according to a key.
		/// </summary>
		/// <typeparam name="TKey">The type of the key returned by the function that is 
		/// represented by keySelector.</typeparam>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		ISpecificationResult<TEntity> OrderByAscending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

		/// <summary>
		/// Sorts the elements of a sequence in descending order according to a key.
		/// </summary>
		/// <typeparam name="TKey">The type of the key returned by the function that is 
		/// represented by keySelector.</typeparam>
		/// <param name="keySelector">A function to extract a key from an element.</param>
		ISpecificationResult<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector);

		/// <summary>
		/// Gets the list of domain objects the specification represents.
		/// </summary>
		IList<TEntity> ToList();

		/// <summary>
		/// Gets single domain object the specification represents.
		/// </summary>
		TEntity Single();

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty; 
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		TEntity SingleOrDefault();
	}
}
