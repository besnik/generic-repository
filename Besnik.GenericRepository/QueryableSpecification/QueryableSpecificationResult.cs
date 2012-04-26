using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System;

namespace Besnik.GenericRepository
{
	/// <summary>
	/// Specification result class contains common functionality for filtering result.
	/// </summary>
	/// <typeparam name="TEntity">Domain entity.</typeparam>
	public class QueryableSpecificationResult<TEntity> : ISpecificationResult<TEntity>
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public QueryableSpecificationResult(IQueryable<TEntity> queryable)
		{
			this.Queryable = queryable;
		}

		/// <summary>
		/// Gets or sets IQueryable interface for domain entity.
		/// </summary>
		protected IQueryable<TEntity> Queryable { get; set; }

		/// <summary>
		/// Takes given count of the records represented by the specification.
		/// </summary>
		public ISpecificationResult<TEntity> Take(int count)
		{
			this.Queryable = this.Queryable.Take(count);
			return this;
		}

		/// <summary>
		/// Executes the specification and query behind it and returns list of records
		/// that matches criteria.
		/// </summary>
		public IList<TEntity> ToList()
		{
			return Queryable.ToList();
		}

		/// <summary>
		/// Executes the specification and query behind it and returns the only record
		/// of a sequence. Throws if there is not exactly one element in the sequence.
		/// </summary>
		public TEntity Single()
		{
			return Queryable.Single();
		}


		/// <summary>
		/// Bypasses a specified number of elements in a sequence and then returns 
		/// the remaining elements.
		/// </summary>
		public ISpecificationResult<TEntity> Skip(int count)
		{
			this.Queryable = this.Queryable.Skip(count);
			return this;
		}

		/// <summary>
		/// Sorts the elements of a sequence in ascending order according to a key.
		/// </summary>
		public ISpecificationResult<TEntity> OrderByAscending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
		{
			this.Queryable = this.Queryable.OrderBy(keySelector);
			return this;
		}

		/// <summary>
		/// Sorts the elements of a sequence in descending order according to a key.
		/// </summary>
		public ISpecificationResult<TEntity> OrderByDescending<TKey>(Expression<Func<TEntity, TKey>> keySelector)
		{
			this.Queryable = this.Queryable.OrderByDescending(keySelector);
			return this;
		}

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty; 
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		public TEntity SingleOrDefault()
		{
			return Queryable.SingleOrDefault();
		}
	}
}
