# Business Requirements

* Develop REST endpoints to create, read, update and delete (CRUD) employees using .NET Core WebAPI
* An employee can have some of these properties - firstname, lastname, email address, age etc.
```
  {
    "id": 0,
    "firstName": "string",
    "lastName": "string",
    "email": "user@example.com",
    "age": 0
  }
```
* The combination of first name, last name and email address should be unique.
	```
	The API Returns an Error Code stating a Bad Request when the Email Address matches.
	Returns Status 400 with a message Duplicate Employee Found.
	```
* The application should start with some employees added.
* Provide API documentation (like Swagger) to interact with the REST endpoints.

# Technical Requirements

* Use Entity Framework Core to interact with database
* The database table(s) should be created when the application is started. You can pick any of these approaches:
* * EF code first approach - **Selected**
* * <s>Send the DB script to add the schemas</s>
* * <s>Use in-memory database</s>
* Use Dependency Injection
* * Included Logger as a dependency injection in the Employee Controller.
* Add at least few unit tests
Test cases have been added with Moq framework and also using actual database integration.

# Testing 
Controller Test cases are MOQ
Service Test cases actually rely on database connectivity.

# Configuration
| Name | Purpose | Default |
| -- | -- | -- |
| WipeDB | Clean the DB before every run | True |

