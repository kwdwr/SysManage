# SE 307 Syllabus Management System

[![.NET](https://img.shields.io/badge/.NET-8.0-blue.svg)](https://dotnet.microsoft.com/download/dotnet/8.0)
[![C#](https://img.shields.io/badge/C%23-11.0-green.svg)](https://docs.microsoft.com/en-us/dotnet/csharp/)
[![License](https://img.shields.io/badge/License-Academic-yellow.svg)](LICENSE)

> **Academic Project for SE 307 - Concepts of Object Oriented Programming**  
> **Student:** Kadir Biberoğlu (20200601105)  
> **Instructor:** Kaya Oğuz  
> **Institution:** Izmir University of Economics

## 🎯 Project Overview

A comprehensive syllabus management system demonstrating enterprise-grade object-oriented design patterns, SOLID principles, and clean architecture. The system provides secure CRUD operations for university syllabi with role-based access control, real-time notifications, and version control integration.

## ✨ Key Features

- 🔐 **OAuth Authentication Simulation** with role-based permissions
- 📚 **Complete Syllabus Management** (Create, Read, Update, Delete)
- 👥 **Multi-Role Access Control** (Instructor, Head of Department, Admin)
- 🔔 **Real-time Notification System** using Observer pattern
- 📧 **Email/SMS Notifications** with delivery tracking (Simulated)
- 💾 **JSON-based Persistence** with automatic backup
- 📜 **Version Control** with commit history and diff generation
- 🔙 **Revert Capability** to restore previous syllabus versions
- 📊 **Comprehensive Audit Logging** for all operations
- 🧵 **Thread-safe Concurrent Access** for multi-user scenarios
- 🎮 **Interactive Console Interface** with command pattern

## 🏗️ Architecture & Design Patterns

### Clean Architecture Layers
```
┌─ Presentation Layer ─────────────────────┐
│  Console UI (Program.cs), Commands       │
├─ Application Layer ──────────────────────┤
│  Services (Syllabus, User, Notification) │
├─ Infrastructure Layer ───────────────────┤
│  Repositories (JSON), Adapters (Email)   │
└─ Domain Layer ───────────────────────────┘
   Models (User, Syllabus), Interfaces
```

### Design Patterns Implemented
- **🏭 Factory Pattern** - Centralized service instantiation (`ServiceFactory`)
- **📦 Repository Pattern** - Data access with JSON storage (`JsonDataRepository`)
- **⚡ Command Pattern** - Encapsulating user actions (`ConcreteCommands.cs`)
- **👀 Observer Pattern** - Notification system for changes (`NotificationService`)
- **🔌 Adapter Pattern** - Adapting external channels like Email (`EmailAdapter`)
- **🛡️ Strategy Pattern** - Authorization logic (`AuthorizationService`)

### SOLID Principles
- **Single Responsibility** - Separated concerns (e.g., `SyllabusService` vs `UserManagementService`)
- **Open/Closed** - New commands can be added without modifying the invoker
- **Liskov Substitution** - `InstructorUser` and `AdminUser` can replace `User` base class
- **Interface Segregation** - Specialized interfaces like `INotificationChannel`

## 🚀 Getting Started

### Prerequisites
- [.NET 8.0 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- Docker (Optional, for containerized run)

### Running Locally
```bash
# Data directory will be created automatically on first run

# Run the application
dotnet run --project SyllabusManager/SyllabusManager.csproj
```

### Running with Docker
```bash
# Build the image and run tests
docker build --progress=plain -f SyllabusManager/Dockerfile -t syllabus-app .

# Run the container
docker run --rm -it syllabus-app
```

### First Login
Use these default accounts to explore the system:
```bash
syllabus> login Kaya Oguz      (Pass: kayaoguz123)   # Instructor (CE Dept)
syllabus> login Department Head (Pass: head123)       # Head of Dept (CE)
syllabus> login Admin          (Pass: admin123)      # Administrator (ALL)
```

## 📖 Usage Examples

### Basic Operations
```bash
# View help/menu
syllabus> help

# List all syllabi
syllabus> 1

# Create new syllabus
syllabus> 2
# Follow prompts for Code, Title, Semester

# Update existing syllabus
syllabus> 3

# Delete syllabus
syllabus> 4

# View syllabus details
syllabus> 5

# View commit history
syllabus> 6
```

### Advanced Features
```bash
# Subscribe to notifications (e.g., for "CE" courses)
syllabus> 7

# Revert detailed syllabus to ID
syllabus> 8

# Logout
syllabus> 9
```

### Admin Operations
```bash
# Create new user
syllabus> 10

# Delete user
syllabus> 11
```

## 🧪 Testing

Comprehensive test suite using **xUnit** and **Moq**:

```bash
# Run all tests
dotnet test Tests/Tests.csproj

# Run with coverage (if installed)
dotnet test Tests/Tests.csproj --collect:"XPlat Code Coverage"
```

## 🔒 Security & Permissions

### Role-Based Access Matrix
| Role | View | Create/Edit | Delete | Notifications | Admin Ops |
|------|------|-------------|--------|---------------|-----------|
| Instructor | ✅ All | ✅ Dept Only | ✅ Dept Only | ❌ | ❌ |
| Dept Head | ✅ All | ✅ All | ✅ All | ✅ Watched | ❌ |
| Admin | ✅ All | ✅ All | ✅ All | ✅ All | ✅ |

### Security Features
- **Input Validation** on all commands
- **Audit Trail** (`audit.json`) logging all critical actions
- **Thread-Safety** using locks in Repository

## 📁 Project Structure

```
SysManage/
├── SyllabusManager/          # Main Application
│   ├── Commands/             # User Commands (Command Pattern)
│   ├── Data/                 # JSON Repositories (Repository Pattern)
│   ├── Factories/            # Service Creation (Factory Pattern)
│   ├── Models/               # Domain Models (User, Syllabus)
│   ├── Services/             # Business Logic & Strategies
│   └── Program.cs            # Console Entry Point
├── Tests/                    # Unit Tests
│   ├── CommandTests.cs       # UI Logic Tests
│   └── ServiceTests.cs       # Business Logic Tests
└── README.md                 # This Documentation
```

## 🔧 Technologies Used

- **Language:** C# 11.0 with .NET 8.0
- **Storage:** JSON with file-based persistence
- **Containerization:** Docker
- **Testing:** xUnit, Moq
- **Design Patterns:** Factory, Command, Observer, Adapter, Repository

## 👨‍💻 Author

**Kadir Biberoğlu**  
Student ID: 20200601105  
Course: SE 307 - Concepts of Object Oriented Programming  
Instructor: Kaya Oğuz  
Institution: Izmir University of Economics

---

⭐ **Star this repository if you found it helpful!**
