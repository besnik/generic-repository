Generic-Repository
==================

Generic implementation of Repository pattern in C# .NET

It is lightweight thin layer between domain model and data mappers (e.g. ORMs like NHibernate, Linq2Sql or Entity Framework). The goal is to avoid recreating same repositories over and over again in all projects where repository pattern is used. Designed with respect to DDD (domain driven design). Implements Specification pattern and best used with factory and/or Service locator patter (DI/IoC). 

The generic repository natively supports specification domain pattern that decouples the specification logic (queries) from the repository. It contains extension point for your custom specifications and the specification interface shall be implemented using fluent interface pattern. 

Don't repeat yourself. GenericRepository provides implementation of repository pattern, so you can focus on your domain model and business rules, instead of fighting with specific data mapper technology (some knowledge is still required for doing mapping configuration). You can start loading and saving data to the data storage extremly fast. If the Generic repository does not support requested functionalit natively, you can still very easily use underlying data mapper. 

Nice side effect is that it is very easy to switch between several data mappers, for example from Entity Framework to NHibernate and so on. 

Last but not least: great learning resource. By implementing generic repository with various data mappers, it is very easy to see the design and architecture differences between the libraries. For example, GetById method differs in LinqToSql and NHibernate, because NHibernate accepts type Object as ID, in comparing to LinqToSql that is strongly typed. This small difference makes it more complex to implement base class for LinqToSql repositories as you can see in the sources. Note that IGenericRepository and it's methods are all strongly typed, the difference is just under the hood. So that was one example. Other can be found in the source codes. 

As I was writting and trying to use the generic repository my self for some time, I realized that the complexity of such solution is very high. Having benefit of switching between ORM mappers is not that great. It is good that you wrap your domain queries and filtering using nice fluent interface but you can achieve the same without additional layer. I think it is much faster and resource efficient if you really need to move from one ORM to another - to manually rewrite necessary data access layers rather than maintaining layer like generic repository. The generic repository is still great learning resource how to use queries and filters in DDD, but you must be very careful if you wanna introduce this complexity in your application - even if you are building something similar.

# Contact
Do not hesitate to contact author with any feedback or question:
besnikgeek@gmail.com

# Sources
Download and explore source codes at:
https://github.com/besnik/generic-repository

Back in the old days I used to host source codes on google code and was using Mercurial HG as source versioning system: http://code.google.com/p/genericrepository/

# Test execution
Build solution in Visual Studio and open .nunit file using NUnit GUI. The tests requires some plugins, easiest is to copy \Lib\*.dll to \Besnik.GenericRepository.Tests\bin\Debug\ (or Release).

By default all ORM mappers connects to MSSQL Express database. See .config(s) for connection strings in "Config" directory in Tests project. NHiberate, Linq2Sql and EntityFrameworks uses different files to load configuration. When changing database or connection, make sure you update all connection strings in Config directory!

# Blog
Some additional articles and discussion can be found here:
http://besnikgeek.blogspot.com

# Supported data mappers
NHibernate 3.1.0
LinqToSql .NET 4
Entity Framework CTP4 (waiting till EF 4.1 is officially released)