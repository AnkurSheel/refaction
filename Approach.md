**Project Highlights**

*   Integration Test
*   Analysers
*   Segregation of Concerns by creating separate projects/layers -> WebApi, Services, Model, Data, IOC
*   Repository Pattern 

**Approach**

*   Added Integration tests to ensure that functionality is not broken while refactoring
*   Added analysers to maintain coding standards
*   Moved classes into separate files
*   Avoiding Sql injection not closing of connections and dependencies to ConcreteSqlClasses. Moved connection string to config
*   Refactored the controller into ProductsController and ProductOptionsController
*   Created a Services Project to contain all the business logic.  Created ProductService and ProductOptionsService
*   Created a Models Project to contain the Models
*   Created a Data project to talk to the data store. Implemented a Repository pattern.
*   Renamed and moved refactor-me to Refactor-me.WebApi
*   Removed Products and ProductOptions classes since they just became a container to hold a list of Products and ProductOptions respectively
*   Dependency Injection using SimpleInjector. Created an IOC project so that the WebAPI project did not need to know about Data

**Things That could be done**

*   Use Http Response instead of throwing exceptions
*   ORM
*   Use specification Pattern
*   Unit Tests
*   Add integrity between Product and ProductOption tables
*   Logging
*   Authentication
