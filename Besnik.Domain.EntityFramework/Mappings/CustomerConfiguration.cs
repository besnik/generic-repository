using System;
using System.Data.Entity.ModelConfiguration;

namespace Besnik.Domain.EntityFramework
{
	public class CustomerConfiguration : EntityTypeConfiguration<Customer>
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
			HasKey<int>(c => c.Id);
			Property(c => c.Name).HasMaxLength(255);

			this.Map(
				config => {
					config.Properties(c => new
					{
						Id = c.Id,
						Name = c.Name,
						Age = c.Age
					});

					config.ToTable("Customers");
				}
				);
		}
	}
}
