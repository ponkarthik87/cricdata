# CricClubs Data Sync - Test Suite

This directory contains comprehensive test cases for the CricClubs Data Sync application using xUnit and Shouldly.

## Test Structure

### Configuration Tests
- **CricClubsConfigTests.cs** - Tests for API configuration settings
- **DatabaseConfigTests.cs** - Tests for database configuration settings  
- **SyncOptionsTests.cs** - Tests for synchronization options
- **ConfigurationBindingTests.cs** - Tests for configuration binding from appsettings.json

### Service Tests
- **ConsoleHostedServiceTests.cs** - Tests for the main console hosted service

### Integration Tests
- **ProgramIntegrationTests.cs** - Tests for dependency injection setup and host configuration

### Test Helpers
- **ConfigurationTestHelper.cs** - Helper methods for creating test configurations

## Test Coverage

The test suite covers:

✅ **Configuration Classes**
- Default value initialization
- Property assignment
- Validation of edge cases
- Configuration binding from JSON

✅ **Console Hosted Service**
- Dependency injection
- Logging behavior
- Configuration loading
- Error handling
- Application lifecycle management

✅ **Program Integration**
- Service registration
- Configuration setup
- HTTP client configuration
- Logging configuration
- Environment-specific settings

## Running Tests

```bash
# Run all tests
dotnet test

# Run with detailed output
dotnet test --logger "console;verbosity=normal"

# Run specific test class
dotnet test --filter "CricClubsConfigTests"
```

## Test Frameworks Used

- **xUnit** - Primary testing framework
- **Shouldly** - Fluent assertion library
- **Moq** - Mocking framework for dependencies

## Test Configuration

Tests use:
- In-memory configuration for unit tests
- Mock objects for dependencies
- Test-specific appsettings.test.json for integration tests