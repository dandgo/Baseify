# Baseify -  Clean Architecture Base Project

This project is a .NET 8 backend base project made with the principle of clean architecture and follows principles from DDD and SOLID.

It's entirely based on Milan Jovanovic's pragmatic clean architecture course, but the goal is to improve and customize it over time. I chose this as a base because it uses most of the principles I think are best for enterprise applications, and it's a well-made and solid base.

**This project is a base for other projects**, so it keeps domain objects, business logic, or anything else that other enterprise projects could not reuse at a minimum. However, there are some classes, records, handlers, controllers, or files that are there to serve as examples and can be removed; those files are going to be marked with comments (to be defined)

## Scenarios where this architecture fits well

- If you are using domain-driven design.
- If you have complex business logic like many enterprise applications,
- Highly testable because of how the components are designed. They follow the single responsibility principle and dependency inversion.
- When you have large teams and want to enforce the design.

### Design Principles

These are the most important design principles followed in this project. Most of them came from DDD and SOLID.

- **Separation of concerns.**  A system should be divided into distinct sections, each addressing a separate concern or responsibility; for example, the separation of the domain from external dependencies like a database.
- **Encapsulation.** Hides information from the other components and isolates the components from other components' influence.
- **Dependency inversion.**  High-level modules should not depend on low-level modules; both should depend on abstractions. Abstractions should not depend on details, but details should depend on abstractions.
- **Explicit dependencies.**  A class should explicitly declare its dependencies, usually through its constructor. This makes the code more understandable, testable, and easier to maintain.
- **Single responsability.** Each component should have one reason to change.
- **DRY.** Reduce duplication of code.
- **Persistence ignorance.** Design domain entities so it doesn't matter what persistent technology you use.
- **Bounded context.** Grouping related entities and behavior together.

---

## Layers Overview

![Clean Architecture Diagram](./readme%20content//clean%20architecture%20diagram.png)

### Domain Layer

It's important to note that in DDD, entities encapsulate enterprise logic and business rules. Examples of things you would see inside the domain layer:

- **Entities.** Represent objects with a distinct identity that runs through time and different states. They encapsulate significant business logic and behavior in addition to their data.
- **Value objects.** Value objects are defined by their attributes and don't have a unique identifier. For this project, most of the time, these are represented as records.
- **Domain events.** Used to capture and communicate significant events within a domain. They represent something that has occurred within the domain that may be of interest to other parts of the system
- **Domain services.** Represent operations that are significant to the domain but not naturally tied to any particular entity or value object. They typically involve multiple entities or value objects and provide a way to model domain logic in a clear and organized manner.
- **Interfaces.** Used to separate the implementation between other layers and objects.
- **Exceptions.** Custom exceptions.
- **Enums.** Enumerations necessary for rich domain models.

### Application Layer

Is responsible for:

- **Orchestrates the Domain.**
- **Buissnes logic.** If it does not naturally fit inside the domain layer.
- **Defines the Use Cases.** Takes the domain objects and tells them what to do. Those use cases are implemented in one of two ways:
  - **Application services.** Containing the logic for each Use Case.
  - **CQRS with MediatR.** Splits reading the data from writing the data.

### Infrastructure Layer

Usually, it implements the interfaces defined in the layers below and is responsible for external systems. For example

- **Databases.** Like RDB's, NoSQL.
- **Messaging.** Like RabbitMQ, Kafka or Service Bus.
- **Email providers.**
- **Storage Services.** Like Blob Storage, File Storage.
- **Identity.**

### Presentation Layer

This layer is responsible for defining the system's entry point, passing the request along the system, and presenting the data.

**For this project is a REST API** but can be a gRPC service, SPA, or something else. The most important compoents are:

- **API Endpoints.**
- **Middleware.** Such Authentication, exception handling, logging, and dependency injection setup.

---

## Get Started

### Requirements

- .Net SDK 8
- Visual Studio 2022
- Docker
- Postman

### Optional

To view and edit the UML diagrams in './files/PlantUML Diagrams' folder use VS Code with the following extensions and run the PlantUML Server with docker by following the next instructions.

- Install VS Code Extension PlantUML
- Add this into the vs code settings

``` json
"plantuml.server": "http://localhost:8089/",
"plantuml.render": "PlantUMLServer",
```

- Install VS Code Extension PlantUML Grammar for PlantUML language support.
- Run the container to render the diagrams on local.

``` docker
docker run --name plantuml -d -p 8089:8080 plantuml/plantuml-server:v1.2024.5
```

### Containers - Additional Infrastructure

This project dependeds on the following containers/technologies for persistance, cache, idp, logs.

-**Postgres.** Database
-**Redis.** Cache
-**Keycloack.** Identity Provider (authentication and authorization)
-**Seq.** Structured Logs and traces

### Rename

Replace the word baseify for the name of you project, you can use find and replace feature of visual studio, don't forget appsettings, migrations, dockerfiles, docker compose, and make sure to rename projects too.

Also replace it from the file bookify-realm-export.json inside ./files/keycloack folder and Baseify -v1.0- Endpoints.postman_collection inside ./files/postman folder.

### How to run it

#### Seed Data

**Uncomment Baseify.Api.Program.cs line 43 (app.SeedData())** to seed your database with initial data and then comment it again as this is usually needed just the first time.

At the same level as the .sln file you are going to see **.files\keycloack** folder, inside this folder there is a .json file that has a default template to make an initial setup of keycloack, this file is mounted to the keycloack container through docker-compose and is going to import the initial seetings for keycloack.

Copy paste **appsettings.Development.json** into the API project and copy paste **docket-compose.override.yml** into the Solution folder. Make sure you include them into the solution.

#### Startup Project

Set docker-compose project as Startup Project and then run the project.

Visual Studio and Docker are going to pull and run all the containers needed. You can open docker desktop and watch the runing containers, their urls, open ports, volumnes, etc.

#### First Run

Remember to uncomment the seed data line at Program.cs in the API project.

#### API, Tokens, Postman

You are going to find a postman endopoints collection inside .files/postman, please import it into your postman.

##### Register your user

In postman go to **Users/Register User** post call, replace you parameters in the body tab and send the request. Copy the response which is the Id of the user in our system. You can go to the keycloack admin portal (localhost:18080) to double check or edit your newley registered user.

Then go to **Users/Log in User** and replace your parameters in the body tab, the response is going to be your bearer token.

Replace you bearer token at collection level.

![Bearer Token](./readme%20content/postman%20bearer%20token.png)
