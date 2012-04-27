# Generic Repository
GenericRepository project is generic implementation of Repository pattern in .NET.
For detailed discussion please see [Introduction].


# Lightweight
It is lightweight thin layer between domain model and data mappers (e.g. ORMs like NHibernate, Linq2Sql or Entity Framework). The goal is to avoid recreating same repositories over and over again in all projects where repository pattern is used. Designed with respect to DDD (domain driven design). Implements Filter pattern and best used with factory and/or Service locator patter (DI/IoC). I used the name specification, but it turned out to be confusing of other design pattern, so from now on I call it filters.

Example of usage:

    var customer = new Customer { Name = "Peter Bondra", Age = 37 };
    var specificationLocator = this.IoC.Resolve<ISpecificationLocator>();
    
    using ( var unitOfWork = this.IoC.Resolve<IUnitOfWorkFactory>().BeginUnitOfWork() )
    {
      ICustomerRepository cr = this.IoC.Resolve<ICustomerRepository>(unitOfWork, specificationLocator);
      
      using ( var transaction = unitOfWork.BeginTransaction() )
      {
        cr.Insert(customer);
        transaction.Commit();
      }
    }


# Fluent filter pattern
The generic repository natively supports filter pattern that decouples the filter logic (queries) from the repository. It contains extension point for your custom filters and the filter interface (called specification) shall be implemented using fluent interface pattern.

Example of fluent filter pattern usage (I call it specification in the code):
    ICustomerRepository customerRepository = 
      this.IoC.Resolve<ICustomerRepository>(unitOfWork, specificationLocator);
    
    IList<Customer> = customerRepository.Specify<ICustomerSpecification>()
      .NameStartsWith("Peter")
      .OlderThan(18)
      .ToResult()
      .Take(3)
      .ToList();


# CRUD and DRY
Don't repeat yourself. GenericRepository provides implementation of repository patter, so you can focus on your domain model and business rules, instead of fighting with specific data mapper technology (some knowledge is still required for doing mapping configuration). You can start loading and saving data to the data storage extremly fast. If the Generic repository does not support requested functionalit natively, you can still very easily use underlying data mapper.


# Abstraction of data mappers
Nice sideeffect is that it is very easy to switch between several data mappers, for example from Entity Framework to NHibernate and so on.

# Learning resource
Last but not least: great learning resource. By implementing generic repository with various data mappers, it is very easy to see the design and architecture differences between the libraries. For example, GetById method differs in LinqToSql and NHibernate, because NHibernate accepts type Object as ID, in comparing to LinqToSql that is strongly typed. This small difference makes it more complex to implement base class for LinqToSql repositories as you can see in the sources. Note that IGenericRepository and it's methods are all strongly typed, the difference is just under the hood. So that was one example. Other can be found in the source codes.


# Unit tested
Ships with unit tests.
Note: when running tests, make sure to adapt connection strings in config directory (or data mapper configuration). There is a plan to make default data provider some file based database like sql ce, so the unit tests would work on of the box.

# Well documented
Source codes fully documented.
External documentation and pattern explanation can be found at my [blog](http://besnikgeek.blogspot.com/search/label/generic%20repository).

# Feedback
I would really like to get feedback what features are missing for your project. The layer is very thin and it is easy to extend and add new features. Contributors are welcome.

# Dependencies
The solution file is for Visual Studio 2010 and projects are targeting CLR 4.0 (.NET 4.0);
If there is a need for older version of CLR, it is no problem to build it for CLR 2.0 (.NET 2.0, 3.0, 3.5). Some of the linked libraries may still be compiled for CLR 2.0, the plan is to migrate everything to CLR 4.0.
Unit tests are using NUnit and Mock frameworks.


# Licence
Open to use in commerce and non-commerce projects.

# Supported data mappers
  * NHibernate
  * Linq2Sql
  * Entity Framework 4.0 with EF Feature CTP4

Plan is to implement suppport for
  * NoSql data mappers like Mongo, RavenDb, CouchDB, etc.
  * Anything the community requests

You can always implement IGenericRepository interface for your favourite data mapper and push the implementation.
