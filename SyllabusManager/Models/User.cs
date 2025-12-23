using System;

namespace SyllabusManager.App.Models
{
    // LO4: Inheritance - Base User class
    public abstract class User
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; } // e.g., "CE", "SE"

        public User(string id, string name, string department)
        {
            Id = id;
            Name = name;
            Department = department;
        }

        public abstract string GetRoleDescription();
    }

    public class InstructorUser : User
    {
        public InstructorUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Instructor - Can create and edit syllabi.";
    }

    public class HeadOfDepartmentUser : User
    {
        public HeadOfDepartmentUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Head of Department - Receives notifications for changes.";
    }

    public class AdminUser : User
    {
        public AdminUser(string id, string name, string department)
            : base(id, name, department) { }

        public override string GetRoleDescription() => "Admin - Implementation System Management.";
    }
}
