using System;
using NUnit.Framework;
using System.Configuration;
using System.IO;
using System.Reflection;

namespace Besnik.GenericRepository.Tests
{
	/// <summary>
	/// Base fixture class for injecting unit of work implemenation.
	/// </summary>
	public abstract class BaseUnitOfWorkFixture
	{
		/// <summary>
		/// Constructor.
		/// </summary>
		public BaseUnitOfWorkFixture()
		{
			this.InitializeFactory();
			this.InitializeConfiguration();
		}

		/// <summary>
		/// Gets factory.
		/// </summary>
		protected Factory Factory = null;

		/// <summary>
		/// Gets unit of work factory. The factory usually wrapps an O/R mapper.
		/// </summary>
		protected IUnitOfWorkFactory UnitOfWorkFactory { get; private set; }

		/// <summary>
		/// Gets configuration of config file for the assembly.
		/// </summary>
		protected Configuration Configuration { get; private set; }

		/// <summary>
		/// Creates unit of work factory. Called only once in setup phase of the fixture.
		/// </summary>
		protected abstract IUnitOfWorkFactory CreateUnitOfWorkFactory();

		/// <summary>
		/// Initializes data storage before each test case.
		/// </summary>
		protected abstract void InitializeDataStorage();

		/// <summary>
		/// Initializes factory to build domain entities, mocks, stubs and necessary infrastructure.
		/// </summary>
		protected virtual void InitializeFactory()
		{
			this.Factory = new Factory();
		}

		/// <summary>
		/// Initializes configuration for the assembly where this class resides.
		/// Use configuration to set your connection strings and custom application settings.
		/// </summary>
		protected virtual void InitializeConfiguration()
		{
			var exePath = Path.Combine(
				Environment.CurrentDirectory
				, Assembly.GetExecutingAssembly().ManifestModule.ScopeName
				);

			if (!File.Exists(exePath))
			{
				throw new Exception(
					string.Format("Config file {0} does not exists.", exePath)
					);
			}

			this.Configuration = ConfigurationManager.OpenExeConfiguration(exePath);
		}

		/// <summary>
		/// Setups the fixture.
		/// </summary>
		[TestFixtureSetUp]
		public virtual void SetupFixture()
		{
			this.UnitOfWorkFactory = this.CreateUnitOfWorkFactory();
		}

		/// <summary>
		/// Tears down the fixture.
		/// </summary>
		[TestFixtureTearDown]
		public virtual void TearDownFixture()
		{
			if (this.UnitOfWorkFactory != null)
			{
				this.UnitOfWorkFactory.Dispose();
				this.UnitOfWorkFactory = null;
			}
		}

		/// <summary>
		/// Sets up the test case.
		/// </summary>
		[SetUp]
		public virtual void Setup()
		{
			this.InitializeDataStorage();
		}

		/// <summary>
		/// Tests setup of each test case. See also <see cref="InitializeDataStorage"/> method.
		/// </summary>
		[Test]
		public virtual void CreateSchema()
		{
			// empty, just testing method decorated with NUnit Setup attribute.
		}
	}
}
