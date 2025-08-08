# Copilot Instructions for MicroTasks Codebase

## Big Picture Architecture

- **MicroTasks** is a multi-service solution using .NET Aspire for backend and React+TypeScript+Vite for frontend.
- **Backend**: Each microservice (e.g., Company, Project, WorkItem) lives in its own folder under `services/`, with vertical slice architecture (Models, DTOs, Endpoints, Data, Auth, etc.).
- **Frontend**: The webapp (`microtasks-webapp/`) uses React, MUI, and TypeScript. Service boundaries are reflected in API service files and page components.
- **Data Flow**: RESTful APIs connect frontend and backend. DTOs abstract domain models from API contracts. Auth is handled via Keycloak and Aspire.AuthServices.

## Developer Workflows

- **Build**: Run `dotnet build MicroTasks.sln` for backend; use `pnpm install` and `pnpm dev` for frontend.
- **Run**: Launch backend services via AppHost or individual service projects. Frontend runs with Vite (`pnpm dev`).
- **Test**: Backend tests are in `tests/` (xUnit, AspireHostFixture). Run with `dotnet test`. Frontend tests not present.
- **Debug**: Attach to running .NET service or AppHost; use launch profiles in `Properties/launchSettings.json`.

## Project-Specific Patterns & Conventions

- **Service Structure**: Each service has `Models/`, `Dtos/`, `Endpoints/`, `Data/`, `Auth/`, and `Program.cs`.
- **DTOs**: Always place DTOs in `Dtos/` and keep them simple. Use DTOs for all API payloads.
- **Endpoints**: Use Minimal API (`app.MapGroup`) and group by resource. Enable OpenAPI in dev.
- **Frontend API Services**: Place API logic in `src/services/`, using a `ServiceResult<T>` pattern for error handling (see `companyService.ts`, `projectService.ts`, `workitemService.ts`).
- **Shared Types**: Place reusable types in `src/types/` (e.g., `companyTypes.ts`, `projectTypes.ts`, `ServiceResult.ts`).
- **Error Handling**: Use `ServiceResult<T>` for all service methods, never throw errors directly.
- **Auth**: Use Keycloak for authentication/authorization. Frontend uses `useAuth` for role checks.
- **UI Patterns**: Use MUI DataGrid for listings, dialogs for create/edit, and role-based controls for actions.

## Integration Points & External Dependencies

- **Backend**: Uses EF Core, Npgsql, Aspire.ServiceDefaults, Aspire.AuthServices, Swashbuckle for OpenAPI.
- **Frontend**: Uses React, MUI, Vite, TypeScript, pnpm, and custom hooks for auth.
- **Communication**: RESTful APIs, DTO mapping, and role-based access control.

## Key Files & Directories

- `services/` - Backend microservices (Company, Project, WorkItem)
- `microtasks-webapp/src/pages/` - React pages (CompaniesPage, ProjectsPage)
- `microtasks-webapp/src/services/` - API service files (companyService.ts, projectService.ts, workitemService.ts)
- `microtasks-webapp/src/types/` - Shared types (DTOs, ServiceResult)
- `.github/instructions/` - Project-specific instructions for AI agents
- `README.md` - General project info

## Example Patterns

- **ServiceResult Usage**:
  ```typescript
  // src/services/companyService.ts
  export async function fetchCompanies(): Promise<ServiceResult<Company[]>> {
    const response = await fetch("/companies");
    if (!response.ok) {
      const errorText = await response.text();
      return { success: false, error: errorText };
    }
    const companies = await response.json();
    return { success: true, data: companies };
  }
  ```
- **DTO Placement**:
  - Place all DTOs in `Dtos/` (backend) or `src/types/` (frontend).
- **Minimal API Grouping**:
  - Use `app.MapGroup("/resource")` for endpoint grouping in backend services.

---

If any section is unclear or missing, please provide feedback so instructions can be improved for your team and future AI agents.
