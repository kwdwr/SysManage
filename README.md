# Syllabus Manager System

A comprehensive console-based Syllabus Management System built with .NET 10.0 that demonstrates object-oriented programming principles including inheritance, polymorphism, and design patterns.

## Overview

The Syllabus Manager System is an educational tool designed to manage course syllabi with role-based access control, version control capabilities, and a notification system. It allows instructors to create and modify syllabi while providing department heads and administrators with oversight and notification capabilities.

## Key Features

### Core Functionality
- **Syllabus CRUD Operations**: Create, Read, Update, and Delete course syllabi
- **Flexible Content Structure**: Store syllabus content as flexible JSON-compatible key-value pairs
- **Version Control**: Git-like commit system tracking all changes with diff generation
- **Role-Based Authorization**: Fine-grained access control based on user roles and departments
- **Notification System**: Multi-channel notification system (Email/SMS) using Observer pattern
- **Subscription Management**: Users can subscribe to specific courses or course patterns

### Design Patterns & Principles
- **Inheritance**: Abstract User base class with specialized roles (Instructor, Head of Department, Admin)
- **Polymorphism**: Interface-based notification channels (INotificationChannel)
- **Observer Pattern**: Subscription-based notification system
- **Service Layer Architecture**: Separation of concerns with dedicated service classes
- **Authorization Service**: Centralized authorization logic

## Technology Stack

- **.NET 10.0**: Latest .NET framework
- **C# 13**: Modern C# with nullable reference types and implicit usings
- **System.Text.Json**: Built-in JSON serialization for data persistence
- **File-based Storage**: JSON files for simple, portable data storage

## Project Structure

```
SyllabusManager/
├── Models/
│   ├── DomainModels.cs       # Syllabus, Commit, Subscription models
│   └── User.cs               # User hierarchy (abstract base + concrete roles)
├── Services/
│   ├── CoreServices.cs       # AuthorizationService, VersionControlService
│   ├── SyllabusService.cs    # Main business logic for syllabus operations
│   └── NotificationService.cs # Observer pattern implementation
├── Data/
│   └── DataService.cs        # Data persistence and loading
├── Program.cs                # Entry point and UI
└── SyllabusManager.csproj    # Project configuration
```

## Data Storage

The application stores data in JSON files located in the `bin/Debug/net10.0/Data/` directory:

- `syllabi.json`: All course syllabi
- `commits.json`: Version history of all changes
- `subscriptions.json`: User notification subscriptions
- `users.json`: User data (auto-generated if missing)

## User Roles

### Instructor
- **Permissions**: Create, update, and delete syllabi for their department
- **Department Restriction**: Can only modify courses with their department prefix (e.g., CE instructor can only edit CE courses)
- **Example**: Kaya Oguz (CE Department)

### Head of Department
- **Permissions**: View syllabi and receive notifications
- **Role**: Oversight and monitoring of department course changes
- **Example**: Jane Smith (CE Department)

### Admin
- **Permissions**: Full access to all operations across all departments
- **Role**: System administration and management
- **Example**: Admin (ALL departments)

## Key Features Explained

### Version Control System
The application implements a Git-like version control system:
- **Commits**: Each change creates a commit with:
  - Unique commit ID
  - Author information
  - Timestamp
  - Commit message (the "why")
  - Diff (the "what" changed)
  - Full snapshot of the syllabus state

- **Diff Generation**: Automatically computes differences between versions, tracking changes in:
  - Title
  - Semester
  - Content fields (key-value pairs)

### Authorization System
Fine-grained access control ensures:
- Instructors can only modify courses in their department
- Department matching is done by course code prefix (e.g., "CE221" starts with "CE")
- Admins have unrestricted access
- Delete operations require Instructor or Admin privileges

### Notification System
Multi-channel notification implementation:
- **Observer Pattern**: Users subscribe to course patterns
- **Pattern Matching**: Supports both specific courses ("CE221") and department-wide patterns ("CE")
- **Multiple Channels**: Polymorphic design allows Email and SMS notifications
- **Automatic Triggers**: Notifications sent automatically on create/update operations

### Flexible Content Model
Syllabi can store any structured content:
- Uses `Dictionary<string, object>` for flexible field storage
- Compatible with JSON serialization
- Allows custom fields per syllabus
- No schema restrictions

## Installation & Setup

### Prerequisites
- .NET 10.0 SDK or later
- Windows, macOS, or Linux

### Steps
1. Clone the repository:
   ```bash
   git clone https://github.com/kwdwr/SysManage.git
   cd SysManage
   ```

2. Restore dependencies:
   ```bash
   dotnet restore
   ```

3. Build the project:
   ```bash
   dotnet build
   ```

4. Run the application:
   ```bash
   dotnet run --project SyllabusManager/SyllabusManager.csproj
   ```

## Usage Guide

### Logging In
1. On startup, you'll see a list of available users
2. Enter the number corresponding to your desired user
3. The system will log you in with the appropriate permissions

### Main Menu Options

1. **List Syllabi**: View all available syllabi
2. **Create Syllabus**: Add a new course syllabus
   - Enter course code (e.g., CE221)
   - Enter title and semester
   - Add flexible content fields as key-value pairs
3. **Update Syllabus**: Modify an existing syllabus
   - Provide course code
   - Update any fields (leave blank to keep current value)
   - Enter a commit message explaining the changes
4. **Delete Syllabus**: Remove a syllabus (requires appropriate permissions)
5. **View Syllabus Details**: Display full details of a specific syllabus
6. **View Version History**: See commit history for a course or all courses
7. **Subscribe to Notifications**: Set up notifications for course updates
8. **Logout**: Return to login screen
9. **Exit**: Close the application

### Example Workflow

Creating a new syllabus:
```
Choice: 2
Enter Course Code: CE221
Enter Title: Data Structures
Enter Semester: Fall 2025
Enter content fields (key=value). Enter 'done' to finish.
Key: Credits
Value: 4
Key: Instructor
Value: Kaya Oguz
Key: Prerequisites
Value: CE102
Key: done
```

Updating a syllabus:
```
Choice: 3
Enter Course Code to update: CE221
New Title [Data Structures]: Data Structures and Algorithms
New Semester [Fall 2025]:
Edit Content? (y/n): y
Key: Credits
Value: 5
Key: done
Commit Message: Updated course credits and title
```

### Subscribing to Notifications

Users can subscribe to receive notifications:
```
Choice: 7
Enter User ID to subscribe for (Current: 3):
Enter Course Pattern: CE
```

This subscribes the user to all courses starting with "CE".

## Object-Oriented Features

### Inheritance Example
```csharp
public abstract class User
{
    public abstract string GetRoleDescription();
}

public class InstructorUser : User
{
    public override string GetRoleDescription() => "Instructor - Can create and edit syllabi.";
}
```

### Polymorphism Example
```csharp
public interface INotificationChannel
{
    void Send(string recipient, string message);
}

public class EmailChannel : INotificationChannel { }
public class SmsChannel : INotificationChannel { }
```

### Service Layer Pattern
The application separates concerns into distinct services:
- `AuthorizationService`: Handles permission checks
- `VersionControlService`: Manages commits and diffs
- `NotificationService`: Handles observer pattern and notifications
- `SyllabusService`: Orchestrates all syllabus operations
- `DataService`: Manages data persistence

## Future Enhancements

Potential improvements for the system:
- RESTful API instead of console interface
- Database backend (SQL Server, PostgreSQL)
- Web-based UI
- Real email/SMS integration
- Advanced search and filtering
- Export to PDF
- Approval workflows
- Audit logging
- User authentication with passwords
- Role-based UI customization

## Contributing

This is an educational project demonstrating OOP principles. Contributions are welcome for:
- Bug fixes
- Documentation improvements
- Additional features that demonstrate design patterns
- Unit tests
- Performance optimizations

## License

This project is open-source and available for educational purposes.

## Contact

For questions or feedback, please open an issue on the GitHub repository.

---

**Built with** .NET 10.0 | **Design Patterns**: Observer, Strategy, Service Layer | **Principles**: SOLID, OOP
