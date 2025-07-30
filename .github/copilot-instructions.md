# General Instructions for Copilot

You are an expert .NET Engineer and Architect.
This project is going to be a Microservice-based application using .NET 9.
We are using .NET Aspire as the base framework.
We will need to follow a vertical slice architecture for the microservices.
Make sure to use the appropriate folder structure for the project.

# Copilot Instructions for MicroTasks

## Project Overview

- **MicroTasks** is a .NET Aspire-based solution for managing tasks, structured as a multi-service application.
- Main components:
  - `MicroTasks.AppHost`: Application host and entry point.
  - `services/MicroTasks.Company`: Example service with a `TodoItem` model and CRUD endpoints.
  - `MicroTasks.ServiceDefaults`: Shared service extensions and defaults.

## Architecture & Patterns

- **Service Boundaries**: Each service (e.g., `MicroTasks.Company`) is isolated in its own folder under `services/`.
- **Models**: Domain models (e.g., `TodoItem`) are placed in a `Models/` subfolder within each service.
- **Endpoints**: Minimal API pattern using endpoint groups (`app.MapGroup`) for RESTful operations.
- **Data Storage**: In-memory `ConcurrentDictionary` is used for demo purposes; replace with persistent storage for production.
- **Configuration**: Service-specific settings in `appsettings.json` and `appsettings.Development.json`.

## Developer Workflows

- **Build**: Use `dotnet build MicroTasks.sln` from the workspace root.
- **Run**: Launch services via `dotnet run --project services/MicroTasks.Company` or use the AppHost for orchestration.
- **Debug**: Attach debugger to running service or AppHost; launch profiles in `Properties/launchSettings.json`.
- **Test**: (No test projects detected; add under `tests/` if needed.)

## Conventions & Integration

- **Endpoints**: Grouped by resource (e.g., `/todos` for `TodoItem` CRUD).
- **OpenAPI**: Enabled in development via `app.MapOpenApi()`.
- **Service Defaults**: Use `builder.AddServiceDefaults()` for common configuration.
- **External Dependencies**: NuGet packages managed per project; see `.csproj` files.

## Example: TodoItem CRUD

- Model: `services/MicroTasks.Company/Models/TodoItem.cs`
- Endpoints: Defined in `services/MicroTasks.Company/Program.cs`:
  - `GET /todos` - List all
  - `GET /todos/{id}` - Get by ID
  - `POST /todos` - Create
  - `PUT /todos/{id}` - Update
  - `DELETE /todos/{id}` - Delete

## Recommendations for AI Agents

- Follow the minimal API and endpoint group pattern for new resources.
- Place models in `Models/`, endpoints in `Program.cs`.
- Use in-memory collections for demo; abstract for real data stores.
- Reference `MicroTasks.ServiceDefaults` for shared logic.
- Update this file with new conventions as the project evolves.
