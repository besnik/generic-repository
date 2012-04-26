using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using NHibernate;
using NHibernate.Criterion;

namespace Besnik.GenericRepository.NHibernate
{
	/// <summary>
	/// The class represents criteria specification result with 
	/// generic functionality for all specifications.
	/// </summary>
	public class CriteriaSpecificationResult<TEntity> : ISpecificationResult<TEntity> 
		where TEntity : class
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public CriteriaSpecificationResult(ICriteria criteria)
		{
			this.Criteria = criteria;
		}

		/// <summary>
		/// Gets criteria specification.
		/// </summary>
		protected ICriteria Criteria { get; private set; }

		/// <summary>
		/// Takes only given amount of entities.
		/// </summary>
		public ISpecificationResult<TEntity> Take(int count)
		{
			this.Criteria.SetMaxResults(count);
			return this;
		}

		/// <summary>
		/// Gets list of entities represented by the specification.
		/// </summary>
		public IList<TEntity> ToList()
		{
			return this.Criteria.List<TEntity>();
		}

		/// <summary>
		/// Gets single result represented by the specification.
		/// </summary>
		public TEntity Single()
		{
			var result = this.Criteria.UniqueResult<TEntity>();

			if (result == null)
			{
				throw new InvalidOperationException("The input sequence is empty.");
			}

			return result;
		}

		/// <summary>
		/// Skips given number of entities when returning the result.
		/// </summary>
		public ISpecificationResult<TEntity> Skip(int count)
		{
			this.Criteria.SetFirstResult(count);
			return this;
		}

		/// <summary>
		/// Validates given body of the expression for ordering functionality.
		/// The client has to specify a property or field of the entity by which 
		/// the ordering shall happen.
		/// </summary>
		private void ValidateForMemberExpression(System.Linq.Expressions.Expression expression)
		{
			if (! (expression is MemberExpression) )
			{
				throw new GenericRepositoryException("A property of the entity needs to be specified.");
			}
		}

		/// <summary>
		/// Orders specified entities by given key in ascending order.
		/// </summary>
		public ISpecificationResult<TEntity> OrderByAscending<TKey>(
			System.Linq.Expressions.Expression<Func<TEntity, TKey>> keySelector
			)
		{
			ValidateForMemberExpression(keySelector.Body);

			var propertyName = (keySelector.Body as MemberExpression).Member.Name;
			this.Criteria.AddOrder( Order.Asc(propertyName) );
			return this;
		}

		/// <summary>
		/// Orders specified entities by given key in descending order.
		/// </summary>
		public ISpecificationResult<TEntity> OrderByDescending<TKey>(
			System.Linq.Expressions.Expression<Func<TEntity, TKey>> keySelector
			)
		{
			ValidateForMemberExpression(keySelector.Body);

			var propertyName = (keySelector.Body as MemberExpression).Member.Name;
			this.Criteria.AddOrder( Order.Desc(propertyName) );
			return this;
		}

		/// <summary>
		/// Returns the only element of a sequence, or a default value if the sequence is empty; 
		/// this method throws an exception if there is more than one element in the sequence.
		/// </summary>
		public TEntity SingleOrDefault()
		{
			return this.Criteria.UniqueResult<TEntity>();
		}
	}
}
