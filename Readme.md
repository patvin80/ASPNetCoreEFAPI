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
```
DB Initializer creates the employees. 
WipeDB clean can be used to wipe the employees.
```
* Provide API documentation (like Swagger) to interact with the REST endpoints.
```
Swagger Endpoint locally is accessible at https://localhost:44390/swagger/index.html
```

# Technical Requirements

* Use Entity Framework Core to interact with database
* The database table(s) should be created when the application is started. You can pick any of these approaches:
* * EF code first approach - **Selected**
* * <s>Send the DB script to add the schemas</s>
* * <s>Use in-memory database</s>
* Use Dependency Injection
* * Included EmployeeRepository which is the database access service as a dependency injection in the services and used in Employee Controller.
* Add at least few unit tests
* * Test cases have been added with Moq framework and also using actual database integration.
* Simulate an API Delay
* * Using the Article here - https://robertwray.co.uk/blog/adding-a-delay-to-asp-net-core-web-api-methods-to-simulate-slow-or-erratic-networks

# Installation

## Pre-requisites
* Dotnet Core 6.0 is required.</br>
Justification: Since this is a brand new development decided to go for latest frameworks.
Download and install dotnet core 6 - https://dotnet.microsoft.com/en-us/download/dotnet/6.0

## Developer Instruction
### Visual Studio
1. Clone the repository https://github.com/patvin80/ASPNetCoreEFAPI.git
2. Open LinearAPI.sln
3. Build the Solution
3. Start using the IISExpress
4. Interact with the swagger
5. Run Test cases using the Test Explorer

### Command Prompt
```
git clone https://github.com/patvin80/ASPNetCoreEFAPI.git
cd LinearAPI
dotnet restore
dotnet build
dotnet test
dotnet run
```

Open in Browser - http://localhost:5000/health

# Testing 
Unit Testing and Regression Testing included
* Controller Test cases are MOQ
* Service Test cases actually rely on database connectivity.
* Developer can run the test cases using dotnet test
* Pipeline only runs the MOQ Test cases using dotnet test --filter LinearAPITest.EmployeeServiceTests

# Pipelines
## Build and Test Pipeline
GitHub actions enabled to build and run the test cases when code is checked in.

# Configuration
| Name | Purpose | Default |
| -- | -- | -- |
| WipeDB | Clean the DB before every run | true |
| EnableApiDelay | Simulate the delay | false|

