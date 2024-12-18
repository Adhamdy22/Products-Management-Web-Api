# Store Management

Overview
The Store Management Web API is a backend application built using C# and ASP.NET Core. It provides a robust platform for managing stores, including features for managing categories, items, and user authentication. The project follows a layered architecture with Models, Managers, ViewModels, and Presentation, ensuring modularity, maintainability, and scalability. Authentication and authorization are implemented using JWT (JSON Web Tokens).

Features
User authentication and authorization using JWT.
CRUD operations for categories and items.
Secure endpoints for authenticated users.
Separation of concerns through layered architecture.
Swagger for API documentation.

Technologies Used
C# and ASP.NET Core for backend development.
Entity Framework Core with SQL Server for database management.
JWT for secure user authentication and authorization.
Newtonsoft.Json for JSON serialization and deserialization.
Swagger/OpenAPI for API documentation.

Architecture
The project follows a layered architecture to promote modularity and separation of concerns:

1. Models
Contains entity classes representing the database structure.


2. Managers
Handles business logic and communicates with the data layer (via repositories or DbContext).


3. ViewModels
Contains data transfer objects (DTOs) used for communication between the client and server.


5. Presentation
Contains API controllers that handle HTTP requests and responses.

