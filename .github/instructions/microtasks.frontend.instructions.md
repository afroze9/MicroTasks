---
applyTo: "**/*.ts,**/*.tsx"
---

# Project coding standards for TypeScript and React

## Project Structure & Conventions

- Pages: Place new page components in src/pages/. Use React functional components and organize by feature.
- API Services: Add or update API logic in src/services/. Use the ServiceResult<T> pattern for error handling (see existing service files for examples).
- Types & DTOs: Define new DTOs and shared types in src/types/. Keep them simple and consistent with backend contracts.
- UI Components: Use MUI (Material UI) for all UI elements. Place reusable components in src/components/.
- Auth: Use hooks from src/auth/ (e.g., useAuth) for authentication and role-based access control.

## Feature Development Workflow

1. Plan Feature:

- Break down the feature into UI, API, and types.
- Identify affected pages, services, and types.

2. Implement UI:

- Create or update page/component in src/pages/ or src/components/.
- Use MUI DataGrid for listings, dialogs for create/edit, and role-based controls for actions.

3. API Integration:

- Add new service methods in src/services/ using the ServiceResult<T> pattern.
- Ensure all API payloads use DTOs from src/types/.

4. Types & DTOs:

- Define new types in src/types/ as needed.
- Keep DTOs simple and aligned with backend.

5. Error Handling:

- Always return ServiceResult<T> from service methods.
- Never throw errors directly; handle errors gracefully in UI.

6. Auth & Access Control:

- Use useAuth to check roles and permissions.
- Hide or disable UI actions based on user roles.

7. Testing & Validation:

- Manually test new features in the browser (pnpm dev).
- Ensure error states and edge cases are handled.

## Build & Run

- Install dependencies: pnpm install
- Start dev server: pnpm dev
- Build for production: pnpm build

## Best Practices

- Keep code modular and readable.
- Reuse existing components and patterns.
- Document new types and service methods.
- Follow TypeScript strictness and linting rules.
