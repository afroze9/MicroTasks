# MicroTasks.Company Aspire-xUnit Tests

This project contains Aspire-xUnit tests for the MicroTasks.Company service.

## How to Run

1. Build the solution:
   ```pwsh
   dotnet build ../../MicroTasks.sln
   ```
2. Run the tests:
   ```pwsh
   dotnet test
   ```

## Test Coverage
- Company endpoints: CRUD operations
- Todo endpoints: CRUD operations

## Conventions
- Uses AspireHostFixture for service orchestration
- Minimal API endpoint testing
- FluentAssertions for assertions
- xUnit as the test framework
