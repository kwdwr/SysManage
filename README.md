# Syllabus Manager System

A comprehensive console-based Syllabus Management System built with .NET 8.0 demonstrating advanced Object-Oriented Programming (OOP) principles and Design Patterns in a Distributed System context.

## Overview

The Syllabus Manager System solves the problem of decentralized and inconsistent syllabus management. It provides a robust platform for:
- Standardized syllabus creation (JSON-based).
- Git-like Version Control (commits, diffs, history).
- Role-Based Access Control (RBAC) with hierarchical permissions.
- Multi-channel Notification System (Observer Pattern).
- **Admin System Management**: User creation and deletion capabilities.
- **Unit Testing**: Comprehensive test suite covering commands and services.

## Architecture & Design Patterns

The project strictly adheres to **SOLID Principles** and utilizes the following Design Patterns:

### 1. Command Pattern (`Commands/`)
Decouples the invoker (Menu) from the receiver (Services). Every user action is a concrete command class.
- `ICommand`: Base interface.
- `CreateSyllabusCommand`, `RevertCommand`, `CreateUserCommand`: Encapsulate requests as objects.

### 2. Factory Pattern (`Factories/ServiceFactory.cs`)
Centralizes object creation and dependency injection. It handles the instantiation of complex services like `SyllabusService` and `NotificationService` with their dependencies.
- Ensures the client code does not need to know the specific class to instantiate.

### 3. Observer Pattern (`Services/NotificationService.cs`)
Enables the "Publish-Subscribe" mechanism for notifications.
- **Subject**: `NotificationService`.
- **Observers**: Registered channels (e.g., Email, SMS).
- Users subscribe to course patterns (e.g., "CE*") and are notified automatically on changes.

### 4. Adapter Pattern (`Services/EmailAdapter.cs`)
Allows incompatible interfaces to work together.
- `INotificationChannel` interface abstracts the sending mechanism.
- `EmailAdapter` adapts a potential external email library to our system's interface.

### 5. Service Layer Pattern (`Services/`)
Encapsulates business logic.
- `AuthorizationService`: Central logic for "Who can do what".
- `UserManagementService`: Logic for Admin operations.
- `VersionControlService`: Logic for computing diffs and managing commits.

## Project Structure

```
SysManage/
├── SyllabusManager/          # Main Application
│   ├── Commands/             # Command implementations
│   ├── Data/                 # JSON Repositories
│   ├── Factories/            # Service Factory
│   ├── Models/               # Domain Models (User, Syllabus, Commit)
│   ├── Services/             # Business Logic Services
│   ├── Program.cs            # Entry Point & Menu
│   └── Dockerfile            # Application Container Config
├── Tests/                    # Unit Tests
│   ├── CommandTests.cs       # Tests for user commands
│   ├── ServiceTests.cs       # Tests for business logic
│   └── Tests.csproj          # xUnit Test Project
└── README.md                 # Documentation
```

## Running with Docker (Recommended)

The project includes a single `Dockerfile` that handles building, testing, and running the application in a containerized environment.

### Step 1: Build & Test
This command builds the project and **automatically runs the unit tests**. If tests fail, the image will not be created.

```bash
docker build --progress=plain -f SyllabusManager/Dockerfile -t syllabus-app .
```

### Step 2: Run Application
Once built, run the application interactively:

```bash
docker run --rm -it syllabus-app
```

## User Roles & Permissions

1.  **Instructor (`InstructorUser`)**
    *   Create/Edit/Delete Syllabi (Only for their Department).
    *   Cannot modify other departments' courses.
    *   *Example Login*: Kaya Oguz

2.  **Head of Department (`HeadOfDepartmentUser`)**
    *   **Full Access**: Can Create/Edit/Delete ANY syllabus regardless of department.
    *   Receives notifications for changes.
    *   *Example Login*: Jane Smith

3.  **Admin (`AdminUser`)**
    *   **System Management**: Create and Delete Users.
    *   Full access to all syllabus operations.
    *   *Example Login*: Admin

## Unit Tests

The project includes a comprehensive Test Suite using **xUnit** and **Moq**.
- **Location**: `/Tests` directory.
- **Coverage**:
    - **Commands**: Verifies that user inputs correctly trigger service methods.
    - **Services**: Verifies business rules (e.g., "Instructor cannot edit wrong dept", "Admin can create user").
    - **Integrations**: Tests the notification subscription flow.

## Technologies

- **.NET 8.0**: Core framework.
- **xUnit**: Testing framework.
- **Moq**: Mocking library for unit tests.
- **Docker**: Containerization and CI/CD simulation.
- **JSON**: Lightweight data persistence.
