## BookStore
Welcome to BookStore, a system for managing a bookstore, developed using ASP.NET Core and Next.js.
BookStore provides functionality to manage books, authors, and orders.
## Project Structure
The project consists of the following key components:

- BookStore.Application: Contains the business logic of the application.
- BookStore.Core: Includes core entities and interfaces.
- BookStore.Host: A web API providing endpoints for system interaction.
- BookStore.PostgreSql: Data access implementation using PostgreSQL.
- BookStore.PostgreSql.MigrationService: Background service designed to automatically apply the latest database migration on startup.
- BookStore.Auth: Contains essential classes and logic for implementing authentication and authorization, including JWT-based authentication, role-based access control, and policy-based authorization.
- bookstore.next: A frontend application built with modern web technologies.
<br/>

## Requirements
- .NET 9.0 or higher
- PostgreSQL
- Node.js
