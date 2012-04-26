using System;
using System.Data.Entity.ModelConfiguration;

namespace Besnik.Domain.EntityFramework
{
	public class CustomerConfiguration : EntityConfiguration<Customer>
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		/// <remarks>
		/// Make sure to update also EntityFrameworkCustomerRepositoryFixture.cs
		/// and InitializeDataStorage() method that clears db before each test.
		/// </remarks>
		public CustomerConfiguration()
		{
			Property(c => c.Id).IsIdentity();
			Property(c => c.Name).HasMaxLength(255);

			this.MapHierarchy(
				c => new
				{
					Id = c.Id,
					Name = c.Name,
					Age = c.Age
				}
				).ToTable("Customers");
		}
	}
}
